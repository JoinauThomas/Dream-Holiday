using DreamHoliday.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Security;
using System.Text;

namespace DreamHoliday.Controllers
{
    
    public class MembreController : BaseController
    {
        private static int idMembreBien;
        private static List<listeDeroulante> datesJours()
        {
            List<listeDeroulante> jours = new List<listeDeroulante>();

            jours.Add(new listeDeroulante { id = Resource.jour , valeur = "" });
            for (int i = 1; i<=31; i++)
            {
                if (i < 10)
                    jours.Add(new listeDeroulante { id = "0"+i.ToString(), valeur = "0"+i.ToString() });
                else
                    jours.Add(new listeDeroulante { id = i.ToString(), valeur = i.ToString() });
            }
            return jours;
        }
        private static List<listeDeroulante> datesMois()
        {
            List<listeDeroulante> mois = new List<listeDeroulante>();

            mois.Add(new listeDeroulante { id = Resource.mois, valeur = "" });
            mois.Add(new listeDeroulante { id = Resource.janvier, valeur = "01" });
            mois.Add(new listeDeroulante { id = Resource.fevrier, valeur = "02" });
            mois.Add(new listeDeroulante { id = Resource.mars, valeur = "03" });
            mois.Add(new listeDeroulante { id = Resource.avril, valeur = "04" });
            mois.Add(new listeDeroulante { id = Resource.mai, valeur = "05" });
            mois.Add(new listeDeroulante { id = Resource.juin, valeur = "06" });
            mois.Add(new listeDeroulante { id = Resource.juillet, valeur = "07" });
            mois.Add(new listeDeroulante { id = Resource.aout, valeur = "08" });
            mois.Add(new listeDeroulante { id = Resource.septembre, valeur = "09" });
            mois.Add(new listeDeroulante { id = Resource.octobre, valeur = "10" });
            mois.Add(new listeDeroulante { id = Resource.novembre, valeur = "11" });
            mois.Add(new listeDeroulante { id = Resource.decembre, valeur = "12" });

            return mois;
        }
        private static List<listeDeroulante> datesAnnees()
        {
            int anneeActuelle = DateTime.Now.Year;
            int anneeMin = anneeActuelle - 140;
            List<listeDeroulante> annees = new List<listeDeroulante>();

            annees.Add(new listeDeroulante { id = Resource.annee, valeur = "" });
            for (int i = anneeActuelle; i >= anneeMin; i--)
            {
                annees.Add(new listeDeroulante { id = i.ToString(), valeur = i.ToString() });
            }
            return annees;
        }

        private List<listeDeroulante> ListedatesJours = datesJours();
        private List<listeDeroulante> ListedatesMois = datesMois();
        private List<listeDeroulante> ListedatesAnnees = datesAnnees();

        [HttpGet]
        public static Membre GetMembreByMail(string mail)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Membre moi = new Membre();
                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/MembreAPI/");
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static bool ajoutNewUser(string mail, string psw1, string psw2)
        {
            try
            {
                bool ok;
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/");
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
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpGet]
        public ActionResult InsertNewMembre()
        {
            ViewBag.ListedatesJours = new SelectList(ListedatesJours, "valeur", "id");
            ViewBag.ListedatesMois = new SelectList(ListedatesMois, "valeur", "id");
            ViewBag.ListedatesAnnees = new SelectList(ListedatesAnnees, "valeur", "id");

            return View();
        }

        [HttpPost]
        public ActionResult InsertNewMembre(string dateNaiss, Membre nouveauMembre, HttpPostedFileBase monfichier)
        {
            //if(ModelState.)
            try
            {

                DateTime dateNaissance = DateTime.ParseExact(dateNaiss, "dd/MM/yyyy", null);
                nouveauMembre.dateDeNaissance = dateNaissance;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/MembreAPI/");
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



            catch (Exception ex)
            {
                throw ex;
            }
        }

    

        [HttpGet]
        public ActionResult VoirMonProfile()
        {
            try
            {
                Membre monCpte = (Membre)Session["monCompte"];

                DateTime dateNaissance = monCpte.dateDeNaissance;
                int jourNais = dateNaissance.Day;
                int moisNais = dateNaissance.Month;
                string jour = "";
                string mois = "";
                string annee = dateNaissance.Year.ToString();

                if (jourNais <= 9)
                {
                    jour = "0" + jourNais.ToString();
                }
                else
                {
                    jour = jourNais.ToString();
                }
                if (moisNais <= 9)
                {
                    mois = "0" + moisNais.ToString();
                }
                else
                {
                    mois = moisNais.ToString();
                }

                

                ViewBag.ListedatesJours = new SelectList(ListedatesJours, "valeur", "id", jour);
                ViewBag.ListedatesMois = new SelectList(ListedatesMois, "valeur", "id", mois);
                ViewBag.ListedatesAnnees = new SelectList(ListedatesAnnees, "valeur", "id", annee);




                int idMembre = monCpte.idMembre;

                editProfile moi = GetMembreByIdForProfile(idMembre);


                int nbLoc = GetCountOfMyLocations(idMembre);
                ViewBag.nombreLoc = nbLoc;

                int nbMessage = GetCountOfMyMessages(idMembre);
                ViewBag.nbMsg = nbMessage;

                return View(moi);

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult updateMembre(string dateNaiss, editProfile moi, HttpPostedFileBase monfichier)
        {
            try
            {
                DateTime dateNaissance = DateTime.ParseExact(dateNaiss, "dd/MM/yyyy", null);
                moi.dateDeNaissance = dateNaissance;
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


                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/MembreAPI/");
                    var responseTask = client.PostAsJsonAsync("PostUpdateMembre", moi);

                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        Membre monCpte = (Membre)Session["monCompte"];
                        monCpte.nom = moi.nom;
                        monCpte.prenom = moi.prenom;
                        monCpte.adresse = moi.adresse;
                        monCpte.dateDeNaissance = moi.dateDeNaissance;
                        monCpte.telephone = moi.telephone;
                        monCpte.mail = moi.mail;
                        monCpte.photo = moi.photo;

                        Session["monCompte"] = monCpte;

                        return RedirectToAction("VoirMonProfile", moi);
                    }
                    else
                    {
                        var content = result.Content.ReadAsStringAsync();
                        content.Wait();
                        ModelState.AddModelError(string.Empty, content.Result);

                        return View("VoirMonProfile", moi);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpGet]
        public ActionResult Contact(int idMembre)
        {
            try
            {
                idMembreBien = idMembre;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        [HttpPost]
        public ActionResult Contact(question questionaire)
        {
            try
            {
                // preparation pour recupérer l'id de l'envoyeur si celui ci est connecté
                int id = 0;
                Membre monCpte = (Membre)Session["monCompte"];
                if(monCpte != null)
                {
                    id = monCpte.idMembre;
                }
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        questionaire.idMembre = idMembreBien;
                        idMembreBien = 0;
                        client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/MembreAPI/");
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
            catch (Exception ex)
            {
                throw ex;
            }
            

        }

        [HttpGet]
        public ActionResult mesMessages()
        {
            try
            {
                Membre moi = (Membre)Session["monCompte"];

                List<Message> mesMessages = new List<Message>();
                using (var client = new HttpClient())
                {
                    var token = Request.Cookies["myToken"].Value;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/BienAPI/");
                    var responseTask = client.GetAsync("GetMyMessages?idMembre=" + moi.idMembre);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<Message>>();
                        readTask.Wait();
                        mesMessages = readTask.Result;
                    }
                }
                return View(mesMessages);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpGet]
        public ActionResult detailMessage(int idMessage)
        {
            try
            {
                Message monMessage = GetDetailOfMyMessage(idMessage);
                return PartialView("_DetailMessage", monMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpGet]
        public ActionResult mesLocations()
        {
            try
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

                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/MembreAPI/");
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
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpGet]
        public List<Membre> GetAllMembres()
        {
            try
            {
                List<Membre> mesMembres = new List<Membre>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/MembreAPI/");
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
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpGet]
        public editProfile GetMembreByIdForProfile(int idMembre)
        {
            try
            {
                editProfile moi = new editProfile();
                using (var client = new HttpClient())
                {
                    var token = Request.Cookies["myToken"].Value;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/MembreAPI/");
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
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpGet]
        public int GetCountOfMyLocations(int idMembre)
        {
            try
            {
                int nbDeLocations = 0;
                using (var client = new HttpClient())
                {
                    var token = Request.Cookies["myToken"].Value;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/BienAPI/");
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
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpGet]
        public int GetCountOfMyMessages(int idMembre)
        {
            try
            {
                int nbMessage = 0;
                using (var client = new HttpClient())
                {
                    var token = Request.Cookies["myToken"].Value;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/BienAPI/");
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
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

       

        [HttpGet]
        public Message GetDetailOfMyMessage(int idMessage)
        {
            try
            {
                Message monMessage = new Message();
                using (var client = new HttpClient())
                {
                    var token = Request.Cookies["myToken"].Value;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/BienAPI/");
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
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        
    }
}