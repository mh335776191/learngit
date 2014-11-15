using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OperationData.Common;
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

        public QiuBaiData()
        {
            //HasDataId = LoadDbFormID();
        }

        public override void GetWebData()
        {
            List<IInfoModel> datalist = new List<IInfoModel>();
            string[] tag =
                {
                    "8hr", 
                    "hot", 
                    "imgrank",
                    "late",
                    "history"
                };

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
                InsertData(datalist);
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
                            if (!HasDataId.Contains(model.FormId))
                            {
                                HasDataId.Add(model.FormId);
                                model.Tag = pagetag;
                                list.Add(model);
                                ExecuteAddEvent(string.Format("author:{3}, id:{0},publishdate:{1}, content:{2} ",
                                                              model.FormId, model.PublishDate, model.Content,
                                                              model.Author));
                            }
                            else
                            {
                                ExecuteAddEvent(string.Format("糗事:{0} 已存在", model.FormId));
                            }
                        }
                    }
                }
            }
            return list;
        }
        public override void InsertData(List<IInfoModel> model)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var m in model)
            {
                QiuBaiInfoModel qiubaimodel = m as QiuBaiInfoModel;
                if (qiubaimodel != null)
                {
                    sb.AppendFormat(@"INSERT INTO dbo.JokeDetail
                                                ( FormId ,
                                                  Author ,
                                                  PublishDate ,
                                                  Content ,
                                                  ImgUrl ,
                                                  Tag 
                                                )
                                        VALUES  ( {0} , -- FormId - int
                                                  '{1}' , -- Author - nvarchar(30)
                                                  '{2}' , -- PublishDate - datetime
                                                  '{3}' , -- Content - nvarchar(max)
                                                  '{4}' , -- ImgUrl - varchar(500)
                                                  '{5}'  -- Tag - varchar(10)          
                                                )", qiubaimodel.FormId, qiubaimodel.Author, qiubaimodel.PublishDate,
                                    qiubaimodel.Content, qiubaimodel.ImgUrl, qiubaimodel.Tag);
                }
            }
            string sql = sb.ToString();
            if (!string.IsNullOrWhiteSpace(sql))
            {
                SqlHelper.ExecuteNonQuery(System.Configuration.ConfigurationSettings.AppSettings["JokeDb"],
                                          System.Data.CommandType.Text, sql);
            }
        }

        public override List<int> LoadDbFormID()
        {
            List<int> list = new List<int>();
            string sql = @"SELECT  FormId
                            FROM    dbo.JokeDetail WHERE DATEDIFF(DAY,PublishDate,GETDATE())<10";
            var read = SqlHelper.ExecuteReader(System.Configuration.ConfigurationSettings.AppSettings["JokeDb"], System.Data.CommandType.Text, sql);
            while (read.Read())
            {
                list.Add(int.Parse(read[0].ToString()));
            }
            read.Dispose();
            return list;
        }
    }
}
