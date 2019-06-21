using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using HttpRequest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FuncLib;
using System.Threading;
using WooCommerceNET.WooCommerce.v3;

namespace MyLib
{
    public class Manager
    {
        private RequestHTTP http = new RequestHTTP();
        private HtmlParser parser = new HtmlParser();
        private IHtmlDocument doc;
        public Manager(string User_agent="")
        {
            if (string.IsNullOrWhiteSpace(User_agent))
                User_agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36";
            //http.SetCookieCapacity(500);
            //http.SetCookieMaxLength(5000);
            //http.SetCookiePerDomainCapcity(500);
            http.SetDefaultHeaders(new string[]
            {
                "User-Agent: "+User_agent
            });
        }

        public bool Login(ref Accounts acc, string captcha)
        {
            string get = http.Request("GET", "https://manager.sunfrogshirts.com/Login.cfm");
            Match m = Regex.Match(get, "name=\"botchecker\" value=\"(.*?)\"");
            if (!m.Success) return false;
            FormData form = new FormData();
            form.Add(new FormElement("username", acc.EMAIL));
            form.Add(new FormElement("password", acc.PASS));
            form.Add(new FormElement("g-recaptcha-response", captcha));
            form.Add(new FormElement("login", ""));
            form.Add(new FormElement("botchecker", m.Groups[1].Value));
            Console.WriteLine(form.ToPostString());
            string html = http.Request("POST", "https://manager.sunfrogshirts.com/Login.cfm", null, form.GetData(),false);
            var headers= http.GetResponseHeaders();
            Console.WriteLine(headers.ToString());
            if (!headers.ToString().Contains("dashboard")) return false;
            return true;
        }
        public string Giai_captcha(string key,int timeOut=60)
        {
            RequestHTTP request = new RequestHTTP();
            string Response = request.Request("GET", "http://2captcha.com/in.php?key="+key+ "&method=userrecaptcha&googlekey=6LeilUMUAAAAAMSYwJVVq4C70mYZSy6NJoMb6q0t&pageurl=https://manager.sunfrogshirts.com/Login.cfm");
            Console.WriteLine("Post Captcha: "+Response);
            if (Response.Contains("OK")) Response = Response.Split('|')[1];
            else return "ERROR";
            for(int i = 0; i < timeOut; i++)
            {
                string captcha = request.Request("GET", "http://2captcha.com/res.php?key="+key+ "&action=get&id="+Response);
                Console.WriteLine("Captcha: "+captcha);
                if (captcha.Contains("OK")) return captcha.Split('|')[1];
                Thread.Sleep(3999);
            }
            return "ERROR";
        }
        public Result Upload(RemoteFileInfo file, Template temp)
        {
            Result result = new Result();


            UploadData data = new UploadData();
            data.Category = temp.Category;
            data.Title = temp.Title;
            data.Description = temp.Description;
            data.showStoreFront = temp.showStoreFront;
            data.types = temp.Types;


            return result;
        }

        private void Update(string html, bool hidden1=false, bool hidden2=false, bool hidden3=false)
        {
            if (!hidden1 && !hidden2 && !hidden3)
                return;
            HtmlParser par = new HtmlParser();
            IHtmlDocument document;
            document = par.Parse(html);
            var form = document.QuerySelector("[id='updateArtForm']");
            if(form==null)
            {
                Console.WriteLine("> not found form update");
                return;
            }

            var nodes = form.QuerySelectorAll("input,textarea");
            MultipartFormData formdata = new MultipartFormData();
            foreach(var node in nodes)
            {
                string name = node.GetAttribute("name");
                string value = "";
                //if (name == "ltoRelaunch"|| name== "LimitedTime")
                //    continue;
                if (name == "Description" || name == "Keywords")
                    value = node.TextContent;
                else
                    value = node.GetAttribute("value");
                if (name.Contains("amount"))
                    value = value.Remove(value.Length - 2);
                formdata.Add(new FormElement(name, value));
            }
            formdata.RemoveElements("ltoRelaunch");
            formdata.RemoveElements("LimitedTime");
            if (hidden1)
                formdata.SetValue("ExcludeFromSearch", "1");
            else
                formdata.RemoveElements("ExcludeFromSearch");
            if (hidden2)
                formdata.SetValue("DoNotAllowGoogle", "1");
            else
                formdata.RemoveElements("DoNotAllowGoogle");
            if (hidden3)
                formdata.SetValue("isBlackout", "2");
            else
                formdata.RemoveElements("isBlackout");
            formdata.Add(new FormElement("submit", ""));

            string res = http.Request("POST", "https://manager.hostingrocket.com/my-art-edit.cfm", new string[]
            {
                "Content-Type: multipart/form-data; boundary=----WebKitFormBoundaryffQeAkANeAVRVjmj"
            }, formdata.GetData("----WebKitFormBoundaryffQeAkANeAVRVjmj"));
            //File.WriteAllText("res_update.html", res);
            //File.WriteAllBytes("post.txt", formdata.GetData("----WebKitFormBoundaryffQeAkANeAVRVjmj"));
        }

        private void FindResult(ref Result result, string html,string title)
        {
            HtmlParser par = new HtmlParser();
            IHtmlDocument document;
            document = par.Parse(html);
            var node = document.QuerySelector("[id='title']");
            if (node == null)
            {
                File.WriteAllText("notfound_"+Funcs.GetFileNameToSave(title)+"_" + Funcs.RandomString(10) + ".html", html);
                Console.WriteLine("> not found title");
                return;
            }
            result.Title = node.GetAttribute("value").Trim();
            if (result.Title.ToLower() != title.Trim().ToLower())
            {
                Console.WriteLine(">not mactch title");
                return;
            }

            node = document.QuerySelector("[id='basic-addon1']");
            if (node == null)
            {
                Console.WriteLine(">not found link");
                return;
            }
            result.Link = (node.Parent.ChildNodes[3] as AngleSharp.Dom.Html.IHtmlInputElement).Value;

            if (string.IsNullOrWhiteSpace(result.Image))
            {
                node = document.QuerySelector("[class='img-responsive thumbnail']");
                if (node == null)
                {
                    Console.WriteLine(">not found image");
                    return;
                }
                string image = "https:" + node.GetAttribute("src");
                //image = image.Replace("images.sunfrogshirts.com", "betaimages.sunfrogshirts.com");
                //Match m = Regex.Match(image, "/m_(\\d+\\-\\d+)");
                //if(m.Success)
                //{
                //    image = image.Replace("/m_" + m.Groups[1].Value, "/"+m.Groups[1].Value);
                //}
                result.Image = image;
            }

            //node = document.QuerySelector("[name='GroupID']");
            //if (node == null)
            //{
            //    Console.WriteLine(">not found GroupID");
            //}
            //else
            //{
            //    result.GroupID = node.GetAttribute("value").Trim();
            //}
        }
    }
}
