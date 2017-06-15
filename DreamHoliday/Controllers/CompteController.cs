using DreamHoliday.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace DreamHoliday.Controllers
{
    public class CompteController : BaseController
    {
        // GET: Compte
        [HttpGet]
        public ActionResult connection()
        {
            return View("_connection");
        }
        [HttpPost]
        public JsonResult connection(string userName, string Password)
        {
            try
            {
                Membre moi = new Membre();

                moi = DreamHoliday.Controllers.MembreController.GetMembreByMail(userName);
                if (moi == null)
                {
                    return null;
                }
                else
                {
                    string access_token = "";
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var formContent = new FormUrlEncodedContent(new[]
                        {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", userName),
                    new KeyValuePair<string, string>("password", Password),
                });
                        var responseTask = client.PostAsync("/api/MyGetToken", formContent);
                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (!result.IsSuccessStatusCode)
                        {
                            var responseString = result.Content.ReadAsStringAsync();
                            var res = responseString.Result;

                            return null;
                        }
                        else
                        {
                            var responseString = result.Content.ReadAsStringAsync();
                            responseString.Wait();
                            //get access token from response body
                            var jObject = JObject.Parse(responseString.Result);
                            access_token = jObject.GetValue("access_token").ToString();
                            CookieHeaderValue cookie = new CookieHeaderValue("myToken", access_token);
                        }
                    }
                    HttpCookie myToken = new HttpCookie("myToken");
                    myToken.Value = access_token;
                    Response.Cookies.Add(myToken);

                    Session["monCompte"] = moi;

                    return Json(new { result = "OK", nom = moi.nom, prenom = moi.prenom });
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult deconnexion()
        {
            try
            {
                Session["monToken"] = null;
                Session["monCompte"] = null;

                HttpCookie myToken;
                string cookieName;
                int limit = Request.Cookies.Count;
                for (int i = 0; i < limit; i++)
                {
                    cookieName = Request.Cookies[i].Name;
                    myToken = new HttpCookie(cookieName);
                    myToken.Value = "";
                    Response.Cookies.Add(myToken);
                }


                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}