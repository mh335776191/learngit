using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TaskScheduler;

namespace OperationData.Common
{
    public static class TaskScheulerHelper
    {
        private static bool CheckProcessRunning(string runningProcessname)
        {
            try
            {
                bool returnValue = false;

                Process[] processes = Process.GetProcessesByName(runningProcessname);

                foreach (Process process in processes)
                {
                    returnValue = true;
                    break;
                }

                return returnValue;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateTask(string taskname, string filepath)
        {
            ITaskService taskService = new TaskScheduler.TaskScheduler();

            taskService.Connect();
            List<IRegisteredTask> tasks = new List<IRegisteredTask>();
            // “\\” or @”\” is the RootFolder
            GetData(taskService.GetFolder(@"\"), tasks);
            var checktask = tasks.Where(m => m.Name == taskname).ToList();
            if (!checktask.Any())//如果不存在任务，创建
            {
                CreateTask(filepath, taskname, taskService);
                return false;
            }
            return true;
        }

        private static void CreateTask(string filepath, string taskname, ITaskService taskService)
        {
            ITaskDefinition taskDefinition = taskService.NewTask(0);
            taskDefinition.RegistrationInfo.Description = taskname;
            taskDefinition.RegistrationInfo.Author = "马欢";
            taskDefinition.Settings.Enabled = true;
            taskDefinition.Settings.Hidden = false;
            taskDefinition.Settings.Compatibility = _TASK_COMPATIBILITY.TASK_COMPATIBILITY_V2_1;
            taskDefinition.Settings.DisallowStartIfOnBatteries = false;
            taskDefinition.Settings.RunOnlyIfIdle = false;

            var tt = (ITimeTrigger)taskDefinition.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_TIME);
            tt.Repetition.Interval = "PT30M";//执行频率
            tt.StartBoundary = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"); //开始执行时间

            ITaskFolder rootFolder = taskService.GetFolder(@"\");//’6′ as argument means this task can be created or updated ["CreateOrUpdate" flag]
            IExecAction action = (IExecAction)taskDefinition.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC);
            action.Path = filepath;

            rootFolder.RegisterTaskDefinition(taskname, taskDefinition, 6, null, null, _TASK_LOGON_TYPE.TASK_LOGON_NONE, null);
        }

        /// <summary>
        /// 获取所有的计划任务列表
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="tasks"></param>
        private static void GetData(ITaskFolder folder, List<IRegisteredTask> tasks)
        {
            var tasklist = folder.GetTasks(1);
            foreach (IRegisteredTask task in tasklist)// get all tasks including those which are hidden, otherwise 0
            {
                tasks.Add(task);
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(task);//release COM object
                foreach (ITaskFolder subFolder in folder.GetFolders(1))
                {
                    GetData(subFolder, tasks);
                }
            }
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(folder);
        }
    }
}
