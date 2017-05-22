using DreamHoliday.Helpers;
using DreamHoliday.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DreamHoliday.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult SetCulture(string culture)
        {
            CultureInfo maCulture = Thread.CurrentThread.CurrentCulture;
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            
            


            DateTimeFormatInfo frenchDateTimeFormat = new CultureInfo("fr").DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = frenchDateTimeFormat;
            maCulture = Thread.CurrentThread.CurrentCulture;

            


            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture; // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        public ActionResult Index()
        {
            Session["probleme"] = 0;
            Session["message"] = "";

            CultureInfo maCulture = Thread.CurrentThread.CurrentCulture;

            if (Request.Cookies["_culture"] != null)
            {
                string lg = Server.HtmlEncode(Request.Cookies["_culture"].Value);
            }
            

            
            ViewBag.Title = "Home Page";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(question questionaire)
        {
            
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:56077/api/HomeAPI/");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseTask = client.PostAsJsonAsync("PostQuestion", questionaire);
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Contact");
                    }
                }
            }
            else
            {
                return View();
            }

        }

        //[HttpPost]
        //public ActionResult Contact(string nom, string prenom, string mail, string question)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            question qu = new question { nom = nom, prenom = prenom, message = question, mail = mail };

        //            client.BaseAddress = new Uri("http://localhost:56077/api/HomeAPI/");
        //            var responseTask = client.PostAsJsonAsync("PostQuestion", qu);
        //            responseTask.Wait();
        //            var result = responseTask.Result;

        //            if (result.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //            else
        //            {
        //                return RedirectToAction("Contact");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return View();
        //    }

        //}









    }
}
