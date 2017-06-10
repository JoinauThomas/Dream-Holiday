using DreamHoliday.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace DreamHoliday.Controllers
{
    public class MembreController : BaseController
    {
        private static int idMembreBien;
        

        [HttpGet]
        public static Membre GetMembreByMail(string mail)
        {

            using (var client = new HttpClient())
            {
                Membre moi = new Membre();
                client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
                var responseTask = client.GetAsync("GetMembreByMail?mail=" + mail);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Membre>();
                    readTask.Wait();

                    moi = readTask.Result;
                }
                return moi;
            }
        }
        public static bool ajoutNewUser(string mail, string psw1, string psw2)
        {
            bool ok;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:56077");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", mail),
                    new KeyValuePair<string, string>("Password", psw1),
                    new KeyValuePair<string, string>("ConfirmPassword", psw2)
});
                var responseTask = client.PostAsync("api/Account/Register", formContent);
                responseTask.Wait();
                var result = responseTask.Result;

                if (!result.IsSuccessStatusCode)
                {
                    var responseString = result.Content.ReadAsStringAsync();
                    var res = responseString.Result;
                    ok = false;
                }
                else
                {
                    ok = true;
                }
                return ok;
            }
        }


        [HttpGet]
        public ActionResult InsertNewMembre()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertNewMembre(Membre nouveauMembre, HttpPostedFileBase monfichier)
        {
            //DateTime datenaiss = new DateTime(annee, mois, jour);
            //nouveauMembre.dateDeNaissance = datenaiss;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
                var responseTask = client.PostAsJsonAsync("PostNewMembre", nouveauMembre);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    // le membre a été créé :


                    // ajout de l'user
                    bool ok = ajoutNewUser(nouveauMembre.mail, nouveauMembre.password1, nouveauMembre.password2);

                    Membre moi = GetMembreByMail(nouveauMembre.mail);
                    int idMembre = moi.idMembre;

                    if (monfichier != null && monfichier.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/Img/membres"), "photo" + idMembre.ToString() + ".jpg");
                        monfichier.SaveAs(path);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var content = result.Content.ReadAsStringAsync();
                    content.Wait();
                    ModelState.AddModelError(string.Empty, content.Result);

                    return View(nouveauMembre);
                }

            }
        }

        [HttpGet]
        public ActionResult VoirMonProfile()
        {
            Membre monCpte = (Membre)Session["monCompte"];

            string identifiant = monCpte.identifiant;

            editProfile moi = GetMembreByIdForProfile(identifiant);


            int nbLoc = GetCountOfMyLocations(identifiant);
            ViewBag.nombreLoc = nbLoc;

            int nbMessage = GetCountOfMyMessages(identifiant);
            ViewBag.nbMsg = nbMessage;

            return View(moi);
        }

        [HttpPost]
        public ActionResult updateMembre(editProfile moi, HttpPostedFileBase monfichier)
        {
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (monfichier != null && monfichier.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Img/Membres"), "photo" + moi.idMembre.ToString() + ".jpg");
                    monfichier.SaveAs(path);
                }


                client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
                var responseTask = client.PostAsJsonAsync("PostUpdateMembre", moi);

                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("VoirMonProfile");
                }
                else
                {
                    var content = result.Content.ReadAsStringAsync();
                    content.Wait();
                    ModelState.AddModelError(string.Empty, content.Result);

                    return View ("VoirMonProfile", moi);
                }

            }
        }

        [HttpGet]
        public ActionResult Contact(int idMembre)
        {
            idMembreBien = idMembre;
            return View();
        }
        [HttpPost]
        public ActionResult Contact(question questionaire)
        {

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    questionaire.idMembre = idMembreBien;
                    idMembreBien = 0;
                    client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
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

        [HttpGet]
        public ActionResult mesMessages()
        {
            List<Message> mesMessages = GetMyMessages();

            return View(mesMessages);
        }

        [HttpGet]
        public ActionResult detailMessage(int idMessage)
        {
            Message monMessage = GetDetailOfMyMessage(idMessage);
            return PartialView("_DetailMessage", monMessage);
        }

        [HttpGet]
        public ActionResult mesLocations()
        {
            Membre moi = (Membre)Session["monCompte"];
            int idMembre = moi.idMembre;
            string identifiant = moi.identifiant;

            List<MesLocations> mesLocations = new List<MesLocations>();

            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
                var responseTask = client.GetAsync("GetMyLocations?identifiant=" + identifiant);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<MesLocations>>();
                    readTask.Wait();

                    mesLocations = readTask.Result;
                }
            }

            ViewBag.idMembre = idMembre;

            return View(mesLocations);
        }

        [HttpGet]
        public List<Membre> GetAllMembres()
        {
            List<Membre> mesMembres = new List<Membre>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
                var responseTask = client.GetAsync("GetAllMembres");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Membre>>();
                    readTask.Wait();
                    mesMembres = readTask.Result;
                }
            }
            return mesMembres;
        }

        [HttpGet]
        public editProfile GetMembreByIdForProfile(string identifiant)
        {
            editProfile moi = new editProfile();
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
                var responseTask = client.GetAsync("GetMembreByIdForProfile?identifiant=" + identifiant);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<editProfile>();
                    readTask.Wait();
                    moi = readTask.Result;
                }
            }
            return moi;
        }

        [HttpGet]
        public int GetCountOfMyLocations(string identifiant)
        {
            int nbDeLocations = 0;
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetCountOfMyLocations?identifiant=" + identifiant);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<int>();
                    readTask.Wait();
                    nbDeLocations = readTask.Result;
                }
            }
            return nbDeLocations;
        }

        [HttpGet]
        public int GetCountOfMyMessages(string identifiant)
        {
            int nbMessage = 0;
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetCountOfMyMessages?identifiant=" + identifiant);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<int>();
                    readTask.Wait();
                    nbMessage = readTask.Result;
                }
            }
            return nbMessage;
        }

        [HttpGet]
        public List<Message> GetMyMessages()
        {
            Membre moi = (Membre)Session["monCompte"];
            List<Message> mesMessages = new List<Message>();
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetMyMessages?identifiant=" + moi.identifiant);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Message>>();
                    readTask.Wait();
                    mesMessages = readTask.Result;
                }
            }
            return mesMessages;
        }

        [HttpGet]
        public Message GetDetailOfMyMessage(int idMessage)
        {
            Message monMessage = new Message();
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetDetailOfMyMessage?idMessage=" + idMessage);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Message>();
                    readTask.Wait();
                    monMessage = readTask.Result;
                }
            }
            return monMessage;
        }

    }
}