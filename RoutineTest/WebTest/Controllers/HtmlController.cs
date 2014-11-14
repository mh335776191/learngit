using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace WebTest.Controllers
{
    public class HtmlController : AsyncController
    {
        //
        // GET: /Html/

        //public ActionResult Index()
        //{
        //    return Content("1234");
        //}

        public void IndexAsync()
        {
            AsyncManager.OutstandingOperations.Increment();

            WebRequest request = WebRequest.Create("http://www.baidu.com");
            //启动一个异步的web request
            request.BeginGetResponse(asyncResult =>
            {
                using (WebResponse response = request.EndGetResponse(asyncResult))
                {
                    var stream = response.GetResponseStream();
                    StreamReader read = new StreamReader(stream);
                    string html = read.ReadToEnd();
                    //将结果photoUrls，保存在AsyncManager.Parameters中
                    AsyncManager.Parameters["html"] = html + Thread.CurrentThread.ManagedThreadId;
                    //通知MVC框架操作完成 ，准备调用Completed
                    AsyncManager.OutstandingOperations.Decrement();
                }
            }, null);
        }

        public ActionResult IndexCompleted(string html)
        {
            html += "," + Thread.CurrentThread.ManagedThreadId;
            return Content(html);
        }

        private string _html;
        public ActionResult AsyncTest()
        {
            WebRequest request = WebRequest.Create("http://www.baidu.com");
            request.BeginGetResponse(CallBack, request);
            _html += Thread.CurrentThread.ManagedThreadId;
            Thread.Sleep(2000);
            return Content(_html);
        }

        public void CallBack(IAsyncResult result)
        {
            WebRequest request = result.AsyncState as WebRequest;
            using (WebResponse response = request.EndGetResponse(result))
            {
                var stream = response.GetResponseStream();
                //StreamReader read = new StreamReader(stream);
                //_html = read.ReadToEnd();
                _html += "," + Thread.CurrentThread.ManagedThreadId;
            }

        }
    }
}
