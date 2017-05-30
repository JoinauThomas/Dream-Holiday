using DreamHoliday.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.Mvc;

// 56077

namespace DreamHoliday.Controllers
{
    
    public class BienController : BaseController
    {
        CultureInfo maCulture = Thread.CurrentThread.CurrentCulture;

        //public static string token = "";


        // ajouter un nouveau bien

        [HttpGet]
        public ActionResult addNewBien()
        {
            Membre moi = (Membre)Session["monCompte"];
            int id = moi.idMembre;

            ViewBag.idMembre = id;

            return View();
        }
        [HttpPost]
        public ActionResult addNewBien(Bien nvBien, HttpPostedFileBase monfichier)
        {
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.PostAsJsonAsync("PostNewBien", nvBien);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    // le bien a été créé


                    List<_Bien_id_adresse> BiensListe = new List<_Bien_id_adresse>();

                    List<Bien> mesBiens = new List<Bien>();
                    mesBiens = showAllBiens();


                    //recherche de l'id du nouveau bien

                    foreach (var i in BiensListe)
                    {
                        BiensListe.Add(new _Bien_id_adresse
                        {
                            idBien = i.idBien,
                            numero = i.numero,
                            pays = i.pays,
                            rue = i.rue,
                            ville = i.ville
                        });
                    }

                    _Bien_id_adresse monBien = BiensListe.Find(x => x.pays == nvBien.pays && x.ville == nvBien.ville && x.rue == nvBien.rue && x.numero == nvBien.numero);
                    nvBien.idBien = monBien.idBien;

                    if (monfichier != null && monfichier.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/Img/Biens"), "photo" + nvBien.idBien.ToString() + ".jpg");
                        monfichier.SaveAs(path);
                    }
                }
                else
                {
                    var content = result.Content.ReadAsStringAsync();
                    content.Wait();
                    ModelState.AddModelError(string.Empty, content.Result);

                    return RedirectToAction("nvBien");
                }

                return RedirectToAction("index", "Home");
            }
        }

        public ActionResult EditBien(int idBien)
        {
            // prochaine etape
            Bien monBien = GetBienWithId(idBien);
            return View(monBien);
        }


        // faire une recherche en fonction du lieux et des options

        [HttpGet]
        public ActionResult BigSearchBien()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BigSearchBien(Model_FormBigSearchBien monBien)
        {
            if (monBien.paysOuVille == null)
            {
                monBien.paysOuVille = "";
            }

            using (var client = new HttpClient())
            {
                List<Bien> mesBiensList = new List<Bien>();
                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.PostAsJsonAsync("BigSearchBien", monBien);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Bien>>();
                    readTask.Wait();
                    mesBiensList = readTask.Result;
                    return PartialView("_SearchBiens", mesBiensList);
                }
                // erreur lors de la création de l'étudiant
                var content = result.Content.ReadAsStringAsync();
                content.Wait();
                ModelState.AddModelError(string.Empty, content.Result);
                return View(monBien);
            }
        }



        // recherche les biens disponibles en fonction du lieux, des dates et du nombre de personnes

        [HttpGet]
        public ActionResult SearchBiens()
        {
            List<Bien> mesBiens = new List<Bien>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetAllBiens");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Bien>>();
                    readTask.Wait();
                    mesBiens = readTask.Result;
                }


                
            }

            return View("_SearchBiens", mesBiens);
        }
        [HttpPost]
        public ActionResult SearchBienss(string paysOuVille, string dateDepart, string dateRetour, int nbPers)
        {
            List<Bien> mesBiens = new List<Bien>();

            //DateTime dateDeparts = DateTime.ParseExact(dateDepart, "dd/MM/yyyy", null);
            //DateTime dateRetours = DateTime.ParseExact(dateRetour, "dd/MM/yyyy", null);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("SearchBiens?paysOuVille=" + paysOuVille + "&dateDepart=" + dateDepart + "&dateRetour=" + dateRetour + "&nbPers=" + nbPers);

                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Bien>>();
                    readTask.Wait();
                    mesBiens = readTask.Result;
                }
            }

            return PartialView("_SearchBiens", mesBiens);
        }


        //détail du bien sélectionné

        [HttpGet]
        public ActionResult DetailBien(int idBien)
        {
            Bien monBien = GetBienWithId(idBien);


            List<listeCommentaireDuBien> listeComment = new List<listeCommentaireDuBien>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetListComment?idBien=" + idBien);

                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<listeCommentaireDuBien>>();
                    readTask.Wait();
                    listeComment = readTask.Result;
                }
            }

            ViewBag.comments = listeComment;
            return View(monBien);
        }


        // Louer un Bien

        [HttpGet]
        public ActionResult LouerUnBien(Bien monBien)
        {
            Membre moi = (Membre)Session["monCompte"];

            LocationBien newLoc = new LocationBien { idBien = monBien.idBien, tarifNettoyage = monBien.tarifNettoyage, tarifNuit = monBien.tarifParNuit, idMembre = moi.idMembre };
            ViewBag.probleme = 0;

            return View(newLoc);
        }


        [HttpPost]
        public ActionResult LouerUnBien(string dateArrivee, string dateDepart, int idBien, int idMembre, int Nuit, int Nettoyage, string coutVoyage, string coutTotal, string coutAdmin, int? nbNuits)
        {
            try
            {
                DateTime dateArrivees = DateTime.ParseExact(dateArrivee, "dd/MM/yyyy", null);
                DateTime dateDeparts = DateTime.ParseExact(dateDepart, "dd/MM/yyyy", null);


                using (var client = new HttpClient())
                {
                    var token = Request.Cookies["myToken"].Value;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    LocationBien nvLocation = new LocationBien { dateArrivee = dateArrivees, dateDepart = dateDeparts, idBien = idBien, idMembre = idMembre, tarifNettoyage = Nettoyage, tarifNuit = Nuit, nbNuits = (int)nbNuits, prixSejour = coutVoyage, prixtotal = coutTotal, tarifAdmin = coutAdmin };

                    client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                    var responseTask = client.PostAsJsonAsync("PostNewLocation", nvLocation);
                    responseTask.Wait();
                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View(nvLocation);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        // Montre les biens d'un membre

        [HttpGet]
        public ActionResult VoirMesBiens(int idMembre)
        {
            List<Bien> mesBiens = new List<Bien>();

            mesBiens = GetMesBiens(idMembre);

            return View(mesBiens);
        }

        public ActionResult DetailMonBien(int idBien)
        {
            Bien monBien = GetBienWithId(idBien);
            monBien.noteMoyenne = Math.Round(monBien.noteMoyenne, 2);
            return View(monBien);
        }

        [HttpGet]
        public ActionResult PostCommentAndNote(int idLoc)
        {
            Membre moi = (Membre)Session["monCompte"];


            commentaireEtNote monCommentaire = new commentaireEtNote { idMembre = moi.idMembre, idLocation = idLoc };

            return PartialView("_PostCommentAndNote", monCommentaire);
        }

        [HttpPost]
        public ActionResult PostCommentAndNote(commentaireEtNote monCommEtNote)
        {
            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.PostAsJsonAsync("PostCommentAndNote", monCommEtNote);
                double aaa = monCommEtNote.note;
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("mesLocations", "Membre");
                }

            }
        }


        // a utiliser

        // montre tous les biens

        [HttpGet]
        public List<Bien> showAllBiens()
        {
            HttpCookie monCookie = Request.Cookies["monToken"];
            string token = "";
            if (monCookie != null)
            {
                token = monCookie["monToken"];
            }

            List<Bien> mesBiens = new List<Bien>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetAllBiens");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Bien>>();
                    readTask.Wait();
                    mesBiens = readTask.Result;
                }
            }
            return (mesBiens);
        }

        [HttpGet]
        public Bien GetBienWithId(int idBien)
        {
            Bien monBien = new Bien();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetBienWithId?idBien=" + idBien);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Bien>();
                    readTask.Wait();
                    monBien = readTask.Result;
                }
            }
            return (monBien);
        }

        public List<Bien> GetMesBiens(int idMembre)
        {
            List<Bien> mesBiens = new List<Bien>();

            using (var client = new HttpClient())
            {
                var token = Request.Cookies["myToken"].Value;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GetmesBiens?idMembre=" + idMembre);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Bien>>();
                    readTask.Wait();
                    mesBiens = readTask.Result;
                }
            }
            return mesBiens;
        }



        // récupere les dates pas disponibles
        [HttpGet]
        public JsonResult RechercheDatesPasDispo(int idBien)
        {
            List<DateTime> dates = new List<DateTime>();
            List<string> datesStr = new List<string>();



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
                var responseTask = client.GetAsync("GETDatesPasDispo?idBien=" + idBien);

                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<DateTime>>();
                    readTask.Wait();
                    dates = readTask.Result;
                }
                foreach (var i in dates)
                {
                    datesStr.Add(i.ToShortDateString());
                }

                return Json(new { result = "OK", dates = dates }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult SearchBienss (string paysOuVille, DateTime? dateDepart, DateTime? dateRetour, int nbPers)
        //{
        //    List<Bien> mesBiens = new List<Bien>();
        //    string dateDeparts = dateDepart.Value.ToShortDateString();
        //    string dateRetours = dateRetour.Value.ToShortDateString();
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");
        //        var responseTask = client.GetAsync("SearchBiens?paysOuVille=" + paysOuVille + "&dateDepart=" + dateDeparts + "&dateRetour=" + dateRetours + "&nbPers=" + nbPers);
        //        // COMMENT AJOUTER LES PARAMETRES DANS LE REQUETE???
        //        responseTask.Wait();
        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<List<Bien>>();
        //            readTask.Wait();
        //            mesBiens = readTask.Result;
        //        }

        //        return Json(new { result = "OK", Bien = mesBiens }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpGet]
        public ActionResult showAllBiens22()
        {

            List<Bien> mesBiens = new List<Bien>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/BienAPI/");


                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", "stedesco@ephec.be"),
                    new KeyValuePair<string, string>("password", "stedesco"),
                });
                var responseTaskId = client.PostAsync("/api/MyGetToken", formContent);
                responseTaskId.Wait();
                var resultId = responseTaskId.Result;
                if (!resultId.IsSuccessStatusCode)
                {
                    var responseString = resultId.Content.ReadAsStringAsync();
                    var res = responseString.Result;
                }
                else
                {
                    var responseString = resultId.Content.ReadAsStringAsync();
                    responseString.Wait();
                    //get access token from response body
                    var jObject = JObject.Parse(responseString.Result);
                    string access_token = jObject.GetValue("access_token").ToString();
                    CookieHeaderValue cookie = new CookieHeaderValue("myToken", access_token);
                }

                var responseTask = client.GetAsync("GetAllBiens");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Bien>>();
                    readTask.Wait();
                    mesBiens = readTask.Result;
                }
            }
            return View(mesBiens);
        }

        [HttpGet]
        public ActionResult showAllBiens33()
        {

            List<Bien> mesBiens = new List<Bien>();
            using (var client = new HttpClient())
            {

                string token = (string)Session["monToken"];

                client.BaseAddress = new Uri("http://localhost:56077/api/Values/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var responseTaskLog = client.GetAsync("Values");
                responseTaskLog.Wait();
                var resultLog = responseTaskLog.Result;
                var responseString = resultLog.Content.ReadAsStringAsync();

                var responseTask = client.GetAsync("GetAllBiens");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Bien>>();
                    readTask.Wait();
                    mesBiens = readTask.Result;
                }
            }
            return View("showAllBiens22", mesBiens);
        }

        


    }
}