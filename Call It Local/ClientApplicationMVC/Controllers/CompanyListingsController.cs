using ClientApplicationMVC.Models;

using Messages.DataTypes.Database.CompanyDirectory;
using Messages.ServiceBusRequest.CompanyDirectory.Responses;
using Messages.ServiceBusRequest.CompanyDirectory.Requests;

using System;
using System.Web.Mvc;
using Messages.ServiceBusRequest;

namespace ClientApplicationMVC.Controllers
{
    /// <summary>
    /// This class contains the functions responsible for handling requests routed to *Hostname*/CompanyListings/*
    /// </summary>
    public class CompanyListingsController : Controller
    {
        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings
        /// </summary>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult Index()
        {
            if (Globals.isLoggedIn())
            {
                ViewBag.Companylist = null;
                ViewBag.Index = true;

                ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
                if (connection == null)
                {
                    return RedirectToAction("Index", "Authentication");
                }

                CompanySearchRequest request = new CompanySearchRequest("");
                CompanySearchResponse response = connection.searchCompanyByName(request);

                if (response.result)
                    ViewBag.Companylist = response.list.companyNames;

                return View("Index");
            }
            return RedirectToAction("Index", "Authentication");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/Search
        /// </summary>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult Search(string textCompanyName)
        {
            ViewBag.Index = false;
            if (Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            CompanySearchRequest request = new CompanySearchRequest(textCompanyName);
            CompanySearchResponse response = connection.searchCompanyByName(request);

            if (!response.result)
                ViewBag.SearchResponse = response.response;
            else
                ViewBag.Companylist = response.list.companyNames;

            return View("Index");
        }

        /// <summary>
        /// This function is called when the client navigates to *hostname*/CompanyListings/DisplayCompany/*info*
        /// </summary>
        /// <param name="id">The name of the company whos info is to be displayed</param>
        /// <returns>A view to be sent to the client</returns>
        public ActionResult DisplayCompany(string id)
        {
            if(Globals.isLoggedIn() == false)
            {
                return RedirectToAction("Index", "Authentication");
            }
            if(String.IsNullOrWhiteSpace(id))
            {
                return View("Index");
            }

            ServiceBusConnection connection = ConnectionManager.getConnectionObject(Globals.getUser());
            if (connection == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            ViewBag.CompanyName = id;


            GetCompanyInfoRequest infoRequest = new GetCompanyInfoRequest(new CompanyInstance(id));
            GetCompanyInfoResponse infoResponse = connection.getCompanyInfo(infoRequest);
            ViewBag.CompanyInfo = infoResponse.companyInfo;

            if (infoResponse.result)
            {
                GetCompanyReviewsRequest request = new GetCompanyReviewsRequest(id);
                GetCompanyReviewsResponse result = connection.getReviews(request);
                ViewBag.Reviews = result;
            }
            else
                return View("Index");
           

            return View("DisplayCompany");
        }

        public ActionResult SaveReview(string comment)
        {
            string cameFrom = Request.UrlReferrer.ToString();
            
            if(cameFrom.Split('/').Length == 6 && !String.IsNullOrWhiteSpace(cameFrom.Split('/')[5]))
            {
                SaveReviewRequest req = new SaveReviewRequest(new ReviewModel
                {
                    Review = comment,
                    User = Globals.getUser(),
                    CompanyName = cameFrom.Split('/')[5],
                    Rating = 0
                });

                ServiceBusResponse res = ConnectionManager.getConnectionObject(Globals.getUser()).sendReview(req);
                if(res.result)
                    return RedirectToAction("DisplayCompany", new { id = cameFrom.Split('/')[5] });
                else
                {
                    ViewBag.SaveReviewResponse = res.response;
                    return RedirectToAction("DisplayCompany", new { id = cameFrom.Split('/')[5] });
                }
            }
            else
            {
                return View("Index");
            }            
        }
    }
}