using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperationData.Model;

namespace OperationData
{
    public abstract class BaseData
    {
        public event Action<string> AddEvent;
        /// <summary>
        /// 数据入口
        /// </summary>
        public abstract void GetWebData();
        /// <summary>
        /// 处理html 转换为实体
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public abstract List<IInfoModel> ProcessHtml(string html);
        /// <summary>
        /// 插入数据库
        /// </summary>
        /// <param name="modellist"></param>
        public abstract void InsertData(List<IInfoModel> modellist);

        public abstract List<int> LoadDbFormID();
        public void ExecuteAddEvent(string msg)
        {
            if (AddEvent != null)
                AddEvent(msg);
        }
    }
}
