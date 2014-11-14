using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsTest
{
    public partial class Form1 : Form
    {
        string newids = "";
        private string oldids = "";
        private Regex listReg = new Regex("<div[^>]*class=\"x-grid3-body\"[^>]*>[\\s\\S]*?<input[^>]*?value=\"(?<value>[^>]*?)\"", RegexOptions.IgnoreCase);
        public Form1()
        {
            InitializeComponent();
            //webBrowser1.ScriptErrorsSuppressed = true;
            timer1.Interval = 1000 * 20;
            timer1.Start();
          

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                richTextBox1.Text = (webBrowser1.DocumentText);
                oldids = webBrowser1.DocumentText;
                if (newids.Length + 100 < oldids.Length)
                {
                    newids = oldids.Trim();
                    MessageBox.Show("有新内容");
                }
                webBrowser1.Navigate("https://autodesk.my.salesforce.com/console", false);
            }
            //HtmlElement refresh_bt = webBrowser1.Document.GetElementById("00B30000008rO2j_refresh");
            //if (refresh_bt != null)
            //{
            //    string style = refresh_bt.Style;
            //    if (style.Contains("display:none"))
            //    {
            //        HtmlElement divhtml = webBrowser1.Document.GetElementById("ext-gen3");
            //        string html = divhtml.OuterHtml;
            //        if (!string.IsNullOrWhiteSpace(html))
            //        {
            //            if (listReg.IsMatch(html))
            //            {
            //                newids = listReg.Match(html).Groups["value"].Value;
            //                if (newids != oldids)
            //                {
            //                    MessageBox.Show("有新消息了");
            //                }
            //                oldids = newids;
            //            }
            //        }
            //        refresh_bt.InvokeMember("click");
            //    }
            //}
        }
    }
}
