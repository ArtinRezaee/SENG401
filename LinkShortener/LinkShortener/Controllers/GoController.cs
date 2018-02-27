using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinkShortener.Controllers
{
    public class GoController : Controller
    {
        public Models.Database.LinkDatabase dbContext = Models.Database.LinkDatabase.getInstance();

        public ActionResult Index(string id)
        {
            if (id != string.Empty)
            {
                var url = string.Empty;
                try
                {
                    url = dbContext.getLongUrl(id.ToString());
                }
                catch {
                    return View();
                }
                
                return Redirect(url);
            }
            else
            {
                return View();
            }
        }
    }
}