using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using TXOrm;
using System.Drawing;
namespace RoutineTest
{
    static class Program
    {
        private static System.Timers.Timer timer = new System.Timers.Timer();
        static void Main()
        {
            staticclass.Print();
            Console.ReadKey();
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

        public static void AddNewMethod(this testsealed o)
        {
            Console.WriteLine("密封类扩展方法");
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

        private static void GetDeveloper(int? id)
        {
            using (var db = new kyj_NewHouseDBEntities())
            {
                var deve = db.Premises.First();
                Console.WriteLine(deve.Id);
            }
        }
    }

    public sealed class testsealed
    {
    }

    internal class test
    {
        public void testme()
        {
        }
    }

    internal interface Itest
    {
        void testme();
    }

    internal struct testtruct
    {
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
