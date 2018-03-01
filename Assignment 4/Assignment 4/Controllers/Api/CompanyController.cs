using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Assignment4.Models;

namespace Assignment4.Controllers
{
    public class CompanyController : ApiController
    {
        
        public IEnumerable<Company> GetCompanies()
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

        public Company GetCompany(int id)
        {
            try
            {
                Company comp = Database.getInstance().getCompany(id);

                if (comp != null)
                    return comp;
                else
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (Exception err)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddCompany(Company company)
        {
            try
            {
                if(String.IsNullOrWhiteSpace(company.Name) || String.IsNullOrWhiteSpace(company.Description))
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid format");
                    return response;
                }
                else
                {
                    Database.getInstance().addCompany(company);

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Company has been added");
                    return response;
                }
            }
            catch (Exception err)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, err.Message);
                return response;
            }
        }

    }
}
