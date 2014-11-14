using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperationData.Model
{
    class QiuBaiInfoModel : IInfoModel
    {
        public int FormId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public string ImgUrl { get; set; }
        public DateTime PublishDate { get; set; }
        public string Tag { get; set; }
    }
}
