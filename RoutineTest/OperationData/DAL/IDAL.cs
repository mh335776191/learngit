using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperationData.Model;

namespace OperationData.DAL
{
    public interface IDAL
    {
        List<int> LoadDbFormID();
        void InsertData(List<IInfoModel> model);
        List<DisPlayModel> GetDisPlayList(int index, int pagesize);
    }
}
