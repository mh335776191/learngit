using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using OperationData;
using OperationData.Data;


namespace ProgramData
{
    class Program
    {
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        static void Main(string[] args)
        {
            Console.Title = "获取数据";
            IntPtr intptr = FindWindow("ConsoleWindowClass", "获取数据");
            if (intptr != IntPtr.Zero)
            {
                ShowWindow(intptr, 0);//隐藏这个窗口
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            BaseData qiubai = new QiuBaiData();
            qiubai.AddEvent += (m) => { Console.WriteLine(m); };
            qiubai.GetWebData();
        }

    }

 
}
