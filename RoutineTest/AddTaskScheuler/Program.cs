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
            TaskScheulerHelper.CreateTask("抓取数据计划任务", filepath);//获取数据的计划任务

            string syncalifilepath = System.Environment.CurrentDirectory.Replace("AddTaskScheuler", "SyncAliDate") + "\\SyncAliDate.exe";
            TaskScheulerHelper.CreateTask("更新阿里云站点数据", syncalifilepath);//获取数据的计划任务

            LogHelper.WriteLog("服务信息");
            //LogHelper.WriteLog("异常", new Exception("擦破，报错了！"));
        }
    }
}
