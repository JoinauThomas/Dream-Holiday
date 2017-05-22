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
        public ActionResult allMembres()
        {
            DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
            List<DAL.MEMBRE> mesMem = dbContext.MEMBRE.ToList();
            List<Membre> mesMembres = new List<Membre>();

            foreach (var i in mesMem)
            {
                mesMembres.Add(new Membre { idMembre = i.idMembre, adresse = i.MEM_adresse, dateDeNaissance = i.MEM_dateDeNaissance, estProprietaire = i.MEM_propriétaire, mail = i.MEM_mail, nom = i.MEM_nom, photo = i.MEM_Photo, prenom = i.MEM_prenom, telephone = i.MEM_telephone });
            }

            return View(mesMembres);
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



                    DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
                    List<DAL.MEMBRE> mesMem = dbContext.MEMBRE.ToList();
                    List<Membre> mesMembres = new List<Membre>();

                    foreach (var i in mesMem)
                    {
                        mesMembres.Add(new Membre { idMembre = i.idMembre, adresse = i.MEM_adresse, dateDeNaissance = i.MEM_dateDeNaissance, estProprietaire = i.MEM_propriétaire, mail = i.MEM_mail, nom = i.MEM_nom, photo = i.MEM_Photo, prenom = i.MEM_prenom, telephone = i.MEM_telephone });
                    }


                    Membre aaa = mesMembres.Find(x => x.mail == nouveauMembre.mail);
                    nouveauMembre.idMembre = aaa.idMembre;
                    if (monfichier != null && monfichier.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/Img/membres"), "photo" + nouveauMembre.idMembre.ToString() + ".jpg");
                        monfichier.SaveAs(path);
                        // ajouter le cours
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
            DAL.MEMBRE moi_Db = new DAL.MEMBRE();

            DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
            List<DAL.MEMBRE> mesMem = dbContext.MEMBRE.ToList();

            moi_Db = mesMem.Find(m => m.idMembre == idMembre);
            editProfile moi = new editProfile { idMembre = moi_Db.idMembre, adresse = moi_Db.MEM_adresse, dateDeNaissance = moi_Db.MEM_dateDeNaissance, estProprietaire = moi_Db.MEM_propriétaire, mail = moi_Db.MEM_mail, nom = moi_Db.MEM_nom, photo = moi_Db.MEM_Photo, prenom = moi_Db.MEM_prenom, telephone = moi_Db.MEM_telephone };

            List < DAL.LOCATION > listeLoc = dbContext.LOCATION.ToList();
            List<DAL.LOCATION> MesLocs = dbContext.LOCATION.ToList();

            MesLocs = listeLoc.FindAll(m => m.idMembre == moi.idMembre);
            int nbLoc = MesLocs.Count();
            ViewBag.nombreLoc = nbLoc;

            List<DAL.MESSAGE> mesMess = dbContext.MESSAGE.ToList().FindAll(m => m.MES_MEM_idMembre == moi.idMembre);
            int nbMessage = mesMess.Count;
            ViewBag.nbMsg = nbMessage;

            return View(moi);
        }

        [HttpPost]
        public ActionResult updateMembre(editProfile moi)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:56077/api/MembreAPI/");
                var responseTask = client.PostAsJsonAsync("PostUpdateMembre", moi);
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var content = result.Content.ReadAsStringAsync();
                    content.Wait();
                    ModelState.AddModelError(string.Empty, content.Result);

                    return View(moi);
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
            DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
            List<DAL.MESSAGE> tousLesMessages = dbContext.MESSAGE.ToList().FindAll(x => x.MES_MEM_idMembre == idMembre);
            List<Message> mesMessages = new List<Message>();
            foreach(var m in tousLesMessages)
            {
                mesMessages.Add(new Message { idMembre = m.MES_MEM_idMembre, idMessage = m.idMessage, libelle = m.MES_message, mail = m.MES_mail, nom = m.MES_nom, prenom = m.MES_prenom, dateEnvoi = m.MES_date, sujet = m.MES_Sujet });
            }
           
            return View(mesMessages);
        }

        [HttpGet]
        public ActionResult detailMessage (int idMessage)
        {
            DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
            DAL.MESSAGE LeMessageDB = dbContext.MESSAGE.ToList().Find(x => x.idMessage == idMessage);
            Message monMessage = new Message { idMessage = LeMessageDB.idMessage, dateEnvoi = LeMessageDB.MES_date, idMembre = LeMessageDB.MES_MEM_idMembre,
                libelle = LeMessageDB.MES_message, mail = LeMessageDB.MES_mail, nom = LeMessageDB.MES_nom, prenom = LeMessageDB.MES_prenom, sujet = LeMessageDB.MES_Sujet };
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
        
    }
}