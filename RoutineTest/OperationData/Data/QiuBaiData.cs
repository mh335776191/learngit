using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly Regex DetailReg = new Regex("(<div[^>]*?class=\"author[^>]*>[\\s\\S]*?<img[^>]*alt=\"(?<author>[^\"]*)\"[\\s\\S]*?</div>\\s*)?(?<content><div[^>]*?class=\"content\"\\s*title=\"(?<date>[^>]*)\"[^>]*>[\\s\\S]*?</div>\\s*(<div[^>]*class=\"thumb\">[\\s\\S]*?</div>)?)", RegexOptions.IgnoreCase);
        private readonly Regex ImgReg = new Regex("<img[^>]*?src=\"(?<imgsrc>[^\"]*)\"[^>]*>", RegexOptions.IgnoreCase);
        private readonly Regex htmlreplaceReg = new Regex("<[^>]*>");
        private List<int> HasDataId = new List<int>();
        string pagetag = string.Empty;
        private IDAL Dal = new QiuBaiDal();
        public QiuBaiData()
        {
            HasDataId = Dal.LoadDbFormID();
        }

        public override void GetWebData()
        {
            List<IInfoModel> datalist = new List<IInfoModel>();
            string[] tag =
                {
                    "imgrank",
                    "late",
                    "history",
                    "8hr", 
                    "hot"};
            try
            {
                foreach (string column in tag)
                {
                    int page = 0;
                    datalist.Clear();
                    pagetag = column;
                    string url = MAINURL + "/" + column;
                    string html = HttpHelper.GetHtml(url, null);
                    string nextpage = NextPageReg.Match(html).Groups["nextpage"].Value;
                    while (!string.IsNullOrWhiteSpace(html))
                    {
                        page++;
                        ExecuteAddEvent(string.Format("{0}:第{1}页", column, page));
                        datalist.AddRange(ProcessHtml(html));
                        html = HttpHelper.GetHtml(MAINURL + nextpage, null);
                        nextpage = NextPageReg.Match(html).Groups["nextpage"].Value;
                        if (string.IsNullOrWhiteSpace(nextpage))
                        {
                            datalist.AddRange(ProcessHtml(html));
                            break;
                        }
                    }
                    Dal.InsertData(datalist);
                    LogHelper.WriteLog(string.Format("{0}:获取完毕，共获取到{1}条新数据", column, datalist.Count));
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("糗百数据获取异常", ex);
            }
        }
        public override List<IInfoModel> ProcessHtml(string html)
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
                    if (!HasDataId.Contains(model.FormId))
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
                                model.Content = htmlreplaceReg.Replace(mtc.Groups["content"].Value, "").Replace("'", "");
                                model.Author = mtc.Groups["author"].Value.Replace("'", "");

                                HasDataId.Add(model.FormId);
                                model.Tag = pagetag;
                                list.Add(model);
                                ExecuteAddEvent(string.Format("author:{3}, id:{0},publishdate:{1}, content:{2} ",
                                                              model.FormId, model.PublishDate, model.Content,
                                                              model.Author));

                            }
                        }
                    }
                    else
                    {
                        ExecuteAddEvent(string.Format("糗事:{0} 已存在", model.FormId));
                    }
                }
            }
            return list;
        }



    }
}
