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
        //public ActionResult GetallMembres()
        //{
        //    DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
        //    List<DAL.MEMBRE> mesMem = dbContext.MEMBRE.ToList();
        //    List<Membre> mesMembres = new List<Membre>();

        //    foreach (var i in mesMem)
        //    {
        //        mesMembres.Add(new Membre { idMembre = i.idMembre, adresse = i.MEM_adresse, dateDeNaissance = i.MEM_dateDeNaissance, estProprietaire = i.MEM_propriétaire, mail = i.MEM_mail, nom = i.MEM_nom, photo = i.MEM_Photo, prenom = i.MEM_prenom, telephone = i.MEM_telephone });
        //    }

        //    return View(mesMembres);
        //}
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

            int idMembre = monCpte.idMembre;

            editProfile moi = GetMembreByIdForProfile(idMembre);


            int nbLoc = GetCountOfMyLocations(idMembre);
            ViewBag.nombreLoc = nbLoc;

            int nbMessage = GetCountOfMyMessages(idMembre);
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
                    return RedirectToAction("VoirMonProfile", moi);
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
        public ActionResult mesMessages(int idMembre)
        {
            List<Message> mesMessages = GetMyMessages(idMembre);

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

            List<MesLocations> mesLocations = new List<MesLocations>();

            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
                var responseTask = client.GetAsync("GetMyLocations?idMembre=" + idMembre);
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
        public editProfile GetMembreByIdForProfile(int idMembre)
        {
            editProfile moi = new editProfile();
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
                var responseTask = client.GetAsync("GetMembreByIdForProfile?idMembre=" + idMembre);
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
        public int GetCountOfMyLocations(int idMembre)
        {
            int nbDeLocations = 0;
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetCountOfMyLocations?idMembre=" + idMembre);
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
        public int GetCountOfMyMessages(int idMembre)
        {
            int nbMessage = 0;
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetCountOfMyMessages?idMembre=" + idMembre);
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
        public List<Message> GetMyMessages(int idMembre)
        {
            List<Message> mesMessages = new List<Message>();
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetMyMessages?idMembre=" + idMembre);
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