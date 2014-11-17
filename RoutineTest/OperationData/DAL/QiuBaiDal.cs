using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperationData.Model;

namespace OperationData.DAL
{
    public class QiuBaiDal : IDAL
    {
        readonly string _constr = string.Empty;

        public QiuBaiDal()
        {
            _constr = System.Configuration.ConfigurationSettings.AppSettings["JokeDb"];
        }
        public List<int> LoadDbFormID()
        {
            List<int> list = new List<int>();
            string sql = @"SELECT  FormId
                            FROM    dbo.JokeDetail WHERE DATEDIFF(DAY,PublishDate,GETDATE())<10";
            var read = SqlHelper.ExecuteReader(_constr, System.Data.CommandType.Text, sql);
            while (read.Read())
            {
                list.Add(int.Parse(read[0].ToString()));
            }
            read.Dispose();
            return list;
        }
        public void InsertData(List<IInfoModel> model)
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
                SqlHelper.ExecuteNonQuery(_constr,
                                          System.Data.CommandType.Text, sql);
            }
        }

        public List<DisPlayModel> GetDisPlayList(int index, int pagesize)
        {
            List<DisPlayModel> list = new List<DisPlayModel>();
            string sql = string.Format(@"SELECT  *
                                        FROM    ( SELECT    ROW_NUMBER() OVER ( ORDER BY PublishDate DESC ) RowID ,
                                                            Id ,
                                                            Content ,
                                                            ImgUrl ,
                                                            Tag ,
                                                            PublishDate
                                                  FROM      dbo.JokeDetail
                                                ) temp
                                        WHERE   RowID BETWEEN {0} AND {1}", (index - 1) * pagesize + 1, index * pagesize);
            var reader = SqlHelper.ExecuteReader(_constr, System.Data.CommandType.Text, sql);
            while (reader.Read())
            {
                DisPlayModel model = new DisPlayModel();
                model.Id = Convert.ToInt32(reader["Id"]);
                model.Content = reader["Content"].ToString();
                model.ImgPath = reader["ImgUrl"].ToString();
                model.Tag = reader["Tag"].ToString();
                model.PublishDate = Convert.ToDateTime(reader["PublishDate"]);
                list.Add(model);
            }
            return list;
        }
    }
}
