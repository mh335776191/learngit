using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace OperationData.Common
{
    static class HttpHelper
    {
        /// <summary>
        /// 字典集合转换为url参数
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string GetUrlString(this Dictionary<string, string> dic)
        {
            List<string> list = new List<string>();
            if (dic != null)
            {
                foreach (var v in dic)
                {
                    list.Add(string.Format("{0}={1}", v.Key, v.Value));
                }
            }
            return string.Join("&", list);
        }

        /// <summary>
        /// 获取html源代码
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="parameters">参数字典表</param>
        /// <returns></returns>
        public static string GetHtml(string url, Dictionary<string, string> parameters)
        {
            try
            {
                var paramstr = parameters.GetUrlString();
                if (!string.IsNullOrWhiteSpace(paramstr))
                    url = url + "?" + paramstr;
                HttpWebRequest wReq = (HttpWebRequest)System.Net.WebRequest.Create(url);
                wReq.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.111 Safari/537.36";
               
                wReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                wReq.Headers.Set("Accept-Language", "zh-CN,zh;q=0.8,en-US;q=0.6,en;q=0.4");
                //wReq.Headers.Set("Origin", "XMLHttpRequest");
                //wReq.Headers.Set("Accept-Encoding", "gzip,deflate,sdch");
                CookieContainer cookie = new CookieContainer();
                cookie.SetCookies(new Uri(url, false), "_qqq_uuid_=6beb85657676be58bbcd61e38d950ff48f47c6c6; Hm_lvt_2670efbdd59c7e3ed3749b458cafaa37=1415951669; Hm_lpvt_2670efbdd59c7e3ed3749b458cafaa37=1415951669; bdshare_firstime=1415951669685; __utmt=1; __utma=210674965.892340608.1415951670.1415951670.1415951670.1; __utmb=210674965.1.10.1415951670; __utmc=210674965; __utmz=210674965.1415951670.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); Hm_lvt_3d143f0a07b6487f65609d8411e5464f=1415951670; Hm_lpvt_3d143f0a07b6487f65609d8411e5464f=1415951670");
                wReq.CookieContainer = cookie;
         
            
                wReq.Timeout = 50000;
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
                return "";
            }
            return "";
        }
    }
}
