<<<<<<< HEAD
﻿using System;
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
        public abstract List<IInfoModel> ProcessHtml(string html,string pagetag);
      
        public void ExecuteAddEvent(string msg)
        {
            var tmpevent = AddEvent;
            if (tmpevent != null)
                tmpevent(msg);
        }
    }
}
=======
﻿using System;
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
        public abstract List<IInfoModel> ProcessHtml(string html,string pagetag);
      
        public void ExecuteAddEvent(string msg)
        {
            if (AddEvent != null)
                AddEvent(msg);
        }
    }
}
>>>>>>> 8345cbacd5032a91cb4545f981d0905012804cab
