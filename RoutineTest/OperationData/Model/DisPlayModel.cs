using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperationData.Model
{
    public class DisPlayModel 
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Tag { get; set; }
        public string ImgPath { get; set; }
        public int FormId { get; set; }
        public DateTime PublishDate { get; set; }
        public  string ShowPublishDate {
            get { return PublishDate.ToString("yyyy-M-d HH:mm:ss"); }
        }
    }
}
