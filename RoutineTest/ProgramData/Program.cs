using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperationData;
using OperationData.Data;

namespace ProgramData
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseData qiubai = new QiuBaiData();
            qiubai.AddEvent += (m) => { Console.WriteLine(m); };
            qiubai.GetWebData();
        }
    }
}
