
﻿using System;
﻿using System.Collections.Generic;
﻿using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
﻿using System.Runtime.InteropServices;
﻿using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;

using System.Drawing;
namespace RoutineTest
{
    public class Program
    {

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        static void Main()
        {
            //Console.Title = "获取数据";
            //IntPtr intptr = FindWindow("ConsoleWindowClass", "获取数据");
            //if (intptr != IntPtr.Zero)
            //{
            //    ShowWindow(intptr, 0);//隐藏这个窗口
            //}
            string str = "1232331212121212";
            str = str.Substring(0, str.Length / 3) + "***" + str.Substring((str.Length / 3) * 2, 3);
            Console.WriteLine(str);
            Console.ReadKey();
        }

    }

    public class TestBase
    {
        public void print()
        {
            Console.WriteLine("fu");
        }
    }

    public class TestChild : TestBase
    {
        public void print()
        {
            Console.WriteLine("child");
        }
    }

}