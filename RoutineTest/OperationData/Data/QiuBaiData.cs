using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OperationData.Common;
using OperationData.DAL;
using OperationData.Model;

namespace OperationData.Data
{
    public class QiuBaiData : BaseData
    {
        private const string MAINURL = "http://www.qiushibaike.com";
        private readonly Regex NextPageReg = new Regex("<a[^>]*?href=\"(?<nextpage>[^>]*?)\"[^>]*>\\s*下一页", RegexOptions.IgnoreCase);
        private readonly Regex ListHrefReg = new Regex("<li[^>]*?class=\"comments\"[^>]?>\\s*<a[^>]*?href=\"(?<detailhref>[^>\"]*)\"[^>]*?id=\"c-(?<fromid>\\d+)\"\\s*class=\"qiushi_comments\"", RegexOptions.IgnoreCase);
        private readonly Regex DetailReg = new Regex("(<div[^>]*?class=\"author[^>]*>[\\s\\S]*?<img[^>]*alt=\"(?<author>[^\"]*)\"[\\s\\S]*?</div>\\s*)?\\s*<div[^>]*title=\"下一条\">\\s*(?<content><div[^>]*?class=\"content\"\\s*>[\\s\\S]*?<!--(?<date>[^>]*?)-->\\s*</div>\\s*(<div[^>]*class=\"thumb\">[\\s\\S]*?</div>)?)\\s*</div>\\s*<div[^>]*?class=\"stats\"[^>]*?>[\\s\\S]*?<span[^>]*class=\"number hidden\"[^>]*>(?<goodnum>\\d+)</span>[\\s\\S]*?<span[^>]*?class=\"number hidden\"[^>]*>-(?<replynum>\\d+)</span>", RegexOptions.IgnoreCase);
        private readonly Regex ImgReg = new Regex("<img[^>]*?src=\"(?<imgsrc>[^\"]*)\"[^>]*>", RegexOptions.IgnoreCase);
        private readonly Regex htmlreplaceReg = new Regex("<[^>]*>");
        private List<RepeatModel> HasData = new List<RepeatModel>();
        private object haslock = new object();
        private IDAL Dal = new QiuBaiDal();
        public QiuBaiData()
        {
            HasData = Dal.LoadDbFormID();
        }

        public override void GetWebData()
        {

            string[] tag =
                {
                    "imgrank",
                    "late",
                    "history",
                    "8hr", 
                    "hot"};
            try
            {
                List<Task> tslist = new List<Task>();
                int allsum = 0;
                foreach (string column in tag)
                {
                    string column1 = column;
                    var ts = new Task<int>(k =>
                         {
                             List<IInfoModel> datalist = new List<IInfoModel>();
                             int page = 0;
                             int sumcount = 0;
                             string url = MAINURL + "/" + column1;
                             string html = HttpHelper.GetHtml(url, null);
                             string nextpage = NextPageReg.Match(html).Groups["nextpage"].Value;
                             while (!string.IsNullOrWhiteSpace(html))
                             {
                                 datalist.Clear();//一页一入库
                                 page++;
                                 ExecuteAddEvent(string.Format("{0}:第{1}页", column1, page));
                                 datalist.AddRange(ProcessHtml(html, column1.ToString()));
                                 html = HttpHelper.GetHtml(MAINURL + nextpage, null);
                                 nextpage = NextPageReg.Match(html).Groups["nextpage"].Value;
                                 if (string.IsNullOrWhiteSpace(nextpage))
                                 {
                                     datalist.AddRange(ProcessHtml(html, column1.ToString()));
                                     break;
                                 }
                                 sumcount += datalist.Count;
                                 Dal.InsertData(datalist);
                             }
                             LogHelper.WriteLog(string.Format("{0}:获取完毕，共获取到{1}条新数据", column1, sumcount));
                             return sumcount;
                         }, "");
                    ts.Start();
                    ts.ContinueWith(m =>
                        {
                            allsum += m.Result;
                        });
                    tslist.Add(ts);
                }

                Task.WaitAll(tslist.ToArray());
                LogHelper.WriteLog(string.Format("全部获取完毕,共获取{0}条新数据", allsum));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("糗百数据获取异常", ex);
            }
        }
        public override List<IInfoModel> ProcessHtml(string html, string pagetag)
        {
            List<IInfoModel> list = new List<IInfoModel>();
            if (ListHrefReg.IsMatch(html))
            {
                MatchCollection matchs = ListHrefReg.Matches(html);
                foreach (Match mc in matchs)
                {
                    var model = new QiuBaiInfoModel();
                    int id;
                    int.TryParse(mc.Groups["fromid"].Value, out id);
                    model.FormId = id;
                    var hascount = 0;
                    lock (haslock)
                    {
                        hascount = HasData.Where(m => m.FromId == model.FormId).Count();
                    }
                    if (hascount == 0)//id不靠谱
                    {
                        string detailhref = mc.Groups["detailhref"].Value;
                        if (!string.IsNullOrWhiteSpace(detailhref))
                        {
                            string detailhtml = HttpHelper.GetHtml(MAINURL + detailhref, null);
                            if (DetailReg.IsMatch(detailhtml))
                            {
                                var mtc = DetailReg.Match(detailhtml);
                                DateTime date;
                                DateTime.TryParse(mtc.Groups["date"].Value, out date);
                                model.PublishDate = date;
                                model.ImgUrl = ImgReg.Match(mtc.Groups["content"].Value).Groups["imgsrc"].Value;
                                model.Content = htmlreplaceReg.Replace(mtc.Groups["content"].Value, "").Replace("'", "").Replace("\\r", "").Replace("\\n", "");
                                model.Author = mtc.Groups["author"].Value.Replace("'", "");
                                model.GoodNum = Convert.ToInt32(mtc.Groups["goodnum"].Value);
                                model.ReplyNum = Convert.ToInt32(mtc.Groups["replynum"].Value);
                                lock (haslock)
                                {
                                    hascount = HasData.Where(m => model.Content.Contains(m.Content)).Count();
                                    if (hascount == 0)
                                    {

                                        RepeatModel hsmodel = new RepeatModel();
                                        hsmodel.FromId = model.FormId;
                                        hsmodel.Content = model.Content;
                                        HasData.Add(hsmodel);

                                        model.Tag = pagetag;
                                        list.Add(model);
                                        ExecuteAddEvent(
                                            string.Format("author:{3},赞:{4},回复:{5}, id:{0},publishdate:{1}, content:{2} ",
                                                          model.FormId, model.PublishDate, model.Content,
                                                          model.Author, model.GoodNum, model.ReplyNum));
                                    }
                                    else
                                    {
                                        ExecuteAddEvent(string.Format("糗事:{0}内容部分相似", model.FormId));
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        ExecuteAddEvent(string.Format("糗事:{0}Id 已存在", model.FormId));
                    }
                }
            }
            return list;
        }



    }
}
