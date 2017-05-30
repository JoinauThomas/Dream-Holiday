﻿using DreamHoliday.Models;
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
        public ActionResult connection(string userName, string Password)
        {
            string access_token = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077");
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
                    Session["probleme"] = 1;
                    Session["message"] = "user ou password pas valide\nVeuillez réessayer";
                    return RedirectToAction("connection");
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


            Membre moi = new Membre();

            moi = DreamHoliday.Controllers.MembreController.GetMembreByMail(userName);
            //////// test avec Cookies//////////
            //HttpCookie monToken = new HttpCookie("myToken");
            //monToken["monToken"] = access_token;
            //Response.Cookies.Add(monToken);

            // recupération du token en Session
            HttpCookie myToken = new HttpCookie("myToken");
            myToken.Value = access_token;
            Response.Cookies.Add(myToken);

            Session["monCompte"] = moi;


            return RedirectToAction("Index", "Home");


        }

        public ActionResult deconnexion()
        {
            Session["monToken"] = null;
            Session["monCompte"] = null;


            return RedirectToAction("Index", "Home");
        }
    }
}