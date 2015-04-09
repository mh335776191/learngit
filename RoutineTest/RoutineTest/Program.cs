
﻿using System;
using System.Data.SqlClient;
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
    class Program
    {
        public static event Action<string> TestEventHander;

        private static void OnTestEventHander(string obj)
        {
            Action<string> handler = TestEventHander;
            if (handler != null) handler(obj);
        }

        public void testmethod(Itest test)
        {
            Console.WriteLine("接口");
        }

        public void testmethod(test test)
        {
            Console.WriteLine("子类");
        }
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        static void Main()
        {
            Console.Title = "获取数据";
            IntPtr intptr = FindWindow("ConsoleWindowClass", "获取数据");
            if (intptr != IntPtr.Zero)
            {
                ShowWindow(intptr, 0);//隐藏这个窗口
            }
            
            //Program.TestEventHander += ddd;
            //Program.TestEventHander += ddd;
            //Program.OnTestEventHander("4");
            //Console.WriteLine(tt.GetType());
            //Console.Write(foo(20, 13));
          
            Program p = new Program();
            Itest i = null;
            test t = new test();
            Itest ti = new test();
            p.testmethod(ti);
            Console.ReadKey();
        }

        private static void ddd(string s)
        {
            Console.WriteLine(s);

        }
        static int foo(int x, int y)
        {
            if (x <= 0 || y <= 0)
                return 1;
            int c = foo(x - 6, y / 2);
            int i = 3 * c;
            return i;
        }
        public static string test(string Url)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                wReq.Timeout = 5000;
                // Get the response instance.
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream)
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception(Url + ex.Message);
            }
            return "";
        }
        public static string sKey(string Key)
        {
            int num = 5381;
            int length = Key.Length;
            for (int i = 0; i < length; i++)
            {
                char c = Convert.ToChar(Key.Substring(i, 1));
                num += (num << 5) + (int)c;
            }
            return Convert.ToString(num & 2147483647);
        }
        private static void test(object sender, ElapsedEventArgs e)
        {
            Thread th = new Thread(new ThreadStart(test1));
            th.Start();
        }

        private static void test1()
        {
            Console.WriteLine(DateTime.Now);
        }

    }

    public sealed class testsealed
    {
        public int i = 1;
    }

    internal class test : Itest
    {
        public void testme()
        {
        }
    }

    internal abstract class abstracttest
    {
        public abstract void testme();
    }

    internal interface Itest
    {
        void testme();
    }

    internal struct testtruct : Itest
    {
        public void testme()
        {
            throw new NotImplementedException();
        }
    }

    internal class staticclass
    {
        private static int i = 0;
        static staticclass()
        {
            Console.WriteLine("静态类");
        }

        public static void Print()
        {
            i++;
            Console.WriteLine(i);
        }
    }
}