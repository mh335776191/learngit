using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperationData.Common;


namespace AddTaskScheuler
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath = System.Environment.CurrentDirectory.Replace("AddTaskScheuler", "ProgramData") + "\\ProgramData.exe";
            TaskScheulerHelper.CreateTask("抓取数据服务", filepath);
            LogHelper.WriteLog("服务信息");
            LogHelper.WriteLog("异常", new Exception("擦破，报错了！"));
            
            
        }
    }
}
