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
using Angel
using System.Threading;

namespace SunFrogShirts
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
        public Result Upload(string file, Template temp)
        {
            Result result = new Result();
            UploadResult upload_result = new UploadResult();
            string file_name = Path.GetFileNameWithoutExtension(file);

            UploadData data = new UploadData();
            data.Category = temp.Category;
            data.Title = temp.Title.Replace("{1}", file_name.Replace("_extract", ""));
            data.Description = temp.Description.Replace("{1}", file_name.Replace("_extract", ""));
            data.showStoreFront = temp.showStoreFront;
            data.Collections = temp.Collection.Replace("{1}", file_name.Replace("_extract", ""));

            string[] tmp_kw = temp.Keywords.Replace("{1}", file_name).Split(',');

           // for (int i = 0; i < tmp_kw.Length; i++)
           if(tmp_kw.Length >3 )
            {
                for (int i = 0; i < 3; i++)
                {
                    data.Keywords.Add(tmp_kw[i].Trim().Replace("_extract", ""));
                }

            }

              else
            {
                for(int i = 0; i < tmp_kw.Length; i++)
                {
                    data.Keywords.Add(tmp_kw[i].Trim().Replace("_extract", ""));
                }
            }
           
            if (temp.UploadBack)
            {
                data.imageFront = "";
                data.imageBack = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" id=\"SvgjsSvg1000\" version=\"1.1\" width=\"2400\" height=\"3200\" viewBox=\"311.00000000008 150 387.99999999984004 517.33333333312\"><g id=\"SvgjsG1052\" transform=\"scale(0.15749999999993336 0.15749999999993336) translate(2006.349206350563 1151.7089947094153)\"><image id=\"SvgjsImage1053\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xlink:href=\"__dataURI:0__\" width=\"2400\" height=\"2886\"></image></g><defs id=\"SvgjsDefs1001\"></defs></svg>";
            }
            data.images.Add(new UploadImages(file));
            switch(temp.UploadType)
            {
                case Enums.UploadType.Shirts:
                    //check def type and color
                    UploadTypes def_type = null;
                    switch (temp.DefType)
                    {
                        case Enums.ShirtsType.Guys:
                            def_type = temp.Types.Find(x => x.id == 8);
                            break;
                        case Enums.ShirtsType.Ladies:
                            def_type = temp.Types.Find(x => x.id == 34);
                            break;
                        case Enums.ShirtsType.Youth:
                            def_type = temp.Types.Find(x => x.id == 35);
                            break;
                        case Enums.ShirtsType.Hoodie:
                            def_type = temp.Types.Find(x => x.id == 19);
                            break;
                        case Enums.ShirtsType.Sweatshirt:
                            def_type = temp.Types.Find(x => x.id == 27);
                            break;
                        case Enums.ShirtsType.Guys_VNeck:
                            def_type = temp.Types.Find(x => x.id == 50);
                            break;
                        case Enums.ShirtsType.Ladies_VNeck:
                            def_type = temp.Types.Find(x => x.id == 116);
                            break;
                        case Enums.ShirtsType.Unisex_Tank_Tops:
                            def_type = temp.Types.Find(x => x.id == 118);
                            break;
                        case Enums.ShirtsType.Unisex_Long_Sleeve:
                            def_type = temp.Types.Find(x => x.id == 119);
                            break;
                    }
                    if (def_type != null)
                    {
                        if (def_type.colors.Contains(temp.DefColors))
                        {
                            def_type.colors.Remove(temp.DefColors);
                            def_type.colors.Insert(0, temp.DefColors);
                            temp.Types.Remove(def_type);
                            temp.Types.Insert(0, def_type);
                        }
                    }
                    var all_shirts = temp.Types.FindAll(x => x.id <= 119);
                    data.types.AddRange(all_shirts);
                    break;
                case Enums.UploadType.Leggings:
                    var type_leg = temp.Types.Find(x => x.id == 120);
                    if (type_leg == null)
                        return result;
                    data.types.Add(type_leg);
                    break;
                case Enums.UploadType.Mugs:
                    var all_mugs = temp.Types.FindAll(x => (x.id >= 128) && (x.id <= 145));
                    if (all_mugs == null || all_mugs.Count < 1)
                        return result;
                    data.types.AddRange(all_mugs);
                    break;
                case Enums.UploadType.Posters:
                    var all_posters = temp.Types.FindAll(x => (x.id >= 137) && (x.id <= 140));
                    if (all_posters == null || all_posters.Count < 1)
                        return result;
                    data.types.AddRange(all_posters);
                    break;
                case Enums.UploadType.Cavans:
                    var type_cavans = temp.Types.Find(x => x.id == 143);
                    if (type_cavans == null)
                        return result;
                    data.types.Add(type_cavans);
                    break;
                case Enums.UploadType.Hat:
                    var type_hat = temp.Types.Find(x => x.id == 147);
                    if (type_hat == null)
                        return result;
                    data.types.Add(type_hat);
                    break;
                case Enums.UploadType.Trucker_Cap:
                    var type_trucker_cap = temp.Types.Find(x => x.id == 148);
                    if (type_trucker_cap == null)
                        return result;
                    data.types.Add(type_trucker_cap);
                    break;
            }

            string post = JsonConvert.SerializeObject(data, Formatting.None);
            //File.WriteAllText("post.json", JsonConvert.SerializeObject(data, Formatting.Indented));
            string res = http.Request("POST", "https://manager.sunfrogshirts.com/Designer/php/upload-handler.cfm", new string[]
            {
                "Accept: */*",
                "X-Requested-With: XMLHttpRequest",
                "Content-Type: application/json"
            },Encoding.UTF8.GetBytes(post),true,null,5*60000);
            //File.WriteAllText("res_upload.html", res);
            string getNewDesign = http.Request("GET", "https://manager.sunfrogshirts.com/my-art-edit.cfm?editNewDesign");
            HtmlParser parser = new HtmlParser();
            var doc = parser.Parse(getNewDesign);
            var node = doc.QuerySelector("[class='img-responsive thumbnail']");
            if(node==null)
            {
                Console.WriteLine("not found img");
                result.Image = "";
            }else
            {
                string src = node.GetAttribute("src");
                result.Image = "https:" + src;
            }
            Match m_title = Regex.Match(getNewDesign, "<h1 class=\"m-b-0 m-t-0\">(.*?)<");
            if (!m_title.Success)
            {
                Console.WriteLine("> not found link");
                result.Title= "";
            }
            else
            {
                result.Title = m_title.Groups[1].Value;
            }
            Match m = Regex.Match(getNewDesign, "a href=\"https://www.sunfrog.com/(.*?)html");
            if (!m.Success)
            {
                Console.WriteLine("> not found link");
                result.Link = "";
            }
            else
            {
                result.Link = "https://www.sunfrog.com/" + m.Groups[1].Value+"html";
            }
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
