using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Assignment4.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment4.Controllers
{
    public class CompanyController : ApiController
    {
        // GET: api/values
        public IEnumerable<Company> GetCompany()
        {
            try
            {
                return Database.getInstance().getAllCompanies();
            }
            catch(Exception err)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

    }
}
