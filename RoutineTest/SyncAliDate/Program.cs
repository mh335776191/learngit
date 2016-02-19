using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperationData.Data;

namespace SyncAliDate
{
    class Program
    {
        static void Main(string[] args)
        {
            SynAliData syndata = new SynAliData();
            syndata.UpdateAliJokeDate(30000);
        }
    }
}
