using ClientApplicationMVC.Models;

using Messages.NServiceBus.Commands;
using Messages.DataTypes;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Authentication.Requests;

using System.Web.Mvc;

namespace ClientApplicationMVC.Controllers
{
    /// <summary>
    /// This class contains the functions responsible for handling requests routed to *Hostname*/Authentication/*
    /// </summary>
    public class AuthenticationController : Controller
    {
        /// <summary>
        /// The default method for this controller
        /// </summary>
        /// <returns>The login page</returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Please enter your username and password.";
            return View("Index");
        }

        //This class is incomplete and should be completed by the students in milestone 2
        //Hint: You will need to make use of the ServiceBusConnection class. See EchoController.cs for an example.
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            ServiceBusConnection serviceBusConnection = new ServiceBusConnection(username);
            LogInRequest logInRequest = new LogInRequest(username, password);
             ServiceBusResponse result =serviceBusConnection.sendLogIn(logInRequest);
            ViewBag.LoginResponse = result.response + " " +result.result;

            return View("Index");

        }

        public ActionResult CreateAccount()
        {
            ViewBag.Message = "Please fill out this form to sign up";
            return View("CreateAccount");
        }


        public ActionResult SignUp(string username, string email, string address, string phone, string password, string type)
        {
            AccountType accType;
            switch (type)
            {
                case "user":
                    accType = AccountType.user;
                    break;
                case "business":
                    accType = AccountType.business;
                    break;
                default:
                    accType = AccountType.notspecified;
                    break;
            }

            CreateAccount account = new CreateAccount()
            {
                username = username,
                email = email,
                address = address,
                phonenumber = phone,
                password = password,
                type = accType

            };

            CreateAccountRequest req = new CreateAccountRequest(account);

            try
            {
                ServiceBusResponse response = ConnectionManager.sendNewAccountInfo(req);
                if (response.result)
                    return RedirectToAction("Index", "Home");
                else
                {
                    ViewBag.Message = "Please fill out this form to sign up";
                    ViewBag.CreateAccountResponse = response.response;
                    return View("CreateAccount");
                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine(err.Message);
                ViewBag.Message = "Please fill out this form to sign up";
                ViewBag.CreateAccountResponse = err.Message;
                return View("CreateAccount");
            }
        }


    }
}