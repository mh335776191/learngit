using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OperationData.DAL;
using OperationData.Model;

namespace OperationData.Data
{
    public class SynAliData
    {
        readonly string _localconstr = string.Empty;
        readonly string _remoteconstr = String.Empty;

        public SynAliData()
        {
            _localconstr = System.Configuration.ConfigurationSettings.AppSettings["LocalJokeDb"];
            _remoteconstr = System.Configuration.ConfigurationSettings.AppSettings["RemoteJokeDb"];
        }

        /// <summary>
        /// 更新阿里云虚拟服务器的糗百数据
        /// </summary>
        /// <param name="datasize">最新的条数</param>
        public void UpdateAliJokeDate(int datasize)
        {
            string localsql =
                $@"SELECT TOP {datasize}
                                        *
                                FROM    dbo.JokeDetail
                                ORDER BY Id DESC";
            var localdata = SqlHelper.ExecuteDataset(_localconstr, CommandType.Text, localsql);
            if (localdata?.Tables[0] != null)
            {
                var localtb = localdata.Tables[0];
                List<IInfoModel> list = new List<IInfoModel>();
                foreach (DataRow row in localtb.Rows)
                {
                    QiuBaiInfoModel model = new QiuBaiInfoModel();
                    model.FormId = Convert.ToInt32(row["FormId"]);
                    model.Author = row["Author"].ToString();
                    model.PublishDate = Convert.ToDateTime(row["PublishDate"]);
                    model.Content = row["Content"].ToString();
                    model.ImgUrl = row["ImgUrl"].ToString();
                    model.Tag = row["Tag"].ToString();
                    model.GoodNum = Convert.ToInt32(row["GoodNum"]);
                    model.ReplyNum = Convert.ToInt32(row["ReplyNum"]);
                    list.Add(model);
                }
                TruncateAliJoke();
                IDAL qiubaidal = new QiuBaiDal(_remoteconstr);
                qiubaidal.InsertData(list);
            }
        }
        /// <summary>
        /// 由于容量有限清除数据
        /// </summary>
        private void TruncateAliJoke()
        {
            string truncatesql = @"TRUNCATE TABLE dbo.JokeDetail";
            SqlHelper.ExecuteNonQuery(_remoteconstr, CommandType.Text, truncatesql);
        }
    }
}
