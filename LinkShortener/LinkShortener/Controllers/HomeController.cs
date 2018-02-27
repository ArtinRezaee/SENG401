using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace LinkShortener.Controllers
{
    public class HomeController : Controller
    {
        public Models.Database.LinkDatabase dbContext = Models.Database.LinkDatabase.getInstance();

        public ActionResult Index()
        {
            dbContext.createDB();
            ViewBag.InitialCall = "True";
            return View();
        }

        public ActionResult ShortenUrl(string link)
        {
            
            ViewBag.InitialCall = "False";
            if (!link.ToLower().StartsWith("http://") && !link.ToLower().StartsWith("https://"))
            {
                link = "https://" + link;
            }

            if (string.IsNullOrEmpty(link))
            {
                ViewBag.Error = "True";
                ViewBag.Message = "Invalid URL entered. Please enter a valid URl and try again.";
                return View("Index");
            }
            else if (!IsUrlValid(link))
            {
                ViewBag.Error = "True";
               
                ViewBag.Message = "The URL entered is not valid. URL is unreachable.";
                return View("Index");
            }
            else
            {
                ViewBag.Error = "False";
                ViewBag.Result = string.Format("{0}://{1}{2}/go/{3}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"), dbContext.saveLongURL(link));
                return View("Index");
            }
        }

        private bool IsUrlValid(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 15000;
                request.Method = "HEAD";
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        return response.StatusCode == HttpStatusCode.OK;
                    }
                }
                catch (WebException)
                {
                    
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}