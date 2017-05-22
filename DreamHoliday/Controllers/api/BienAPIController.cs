using DreamHoliday.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DreamHoliday.Controllers.api
{
    [RoutePrefix("api/BienAPI")]
    public class BienAPIController : ApiController
    {

        // ajouter un nouveau bien

        [HttpPost]
        [Route("PostNewBien")]
        public IHttpActionResult PostNewBien(Bien nvBien)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ERREUR, le formulaire a ete mal rempli!!!");
            }
            else
            {
                DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
                dbContext.addNewBien(nvBien.pays, nvBien.ville, nvBien.rue, nvBien.numero, nvBien.idMembre, nvBien.tarifParNuit, 
                    nvBien.tarifNettoyage, nvBien.libelle, nvBien.nbPersonnesMax, nvBien.salleDeBain, nvBien.salon, 
                    nvBien.salleAManger, nvBien.toilette, nvBien.cuisine, nvBien.chambre, nvBien.dressing, nvBien.veranda, 
                    nvBien.bbq, nvBien.piscine, nvBien.jacuzzi, nvBien.sauna, nvBien.tv, nvBien.teleDistribution, nvBien.wifi, 
                    nvBien.pingpong, nvBien.tennis, nvBien.transat, nvBien.cuisineEquipee, nvBien.machineALaver, nvBien.jardin, nvBien.parking);
                return Ok();
            }
        }

        [HttpGet]
        [Route("GetAllBiens")]
        public List<Bien> GetAllBiens()
        {
            DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
            List<DAL.BIEN> BiensListe = dbContext.BIEN.ToList();
            List<Bien> mesBiens = new List<Bien>();


            //recherche de l'id du nouveau bien

            foreach (var i in BiensListe)
            {
                mesBiens.Add(new Bien
                {
                    idBien = i.idBien,
                    idMembre = i.idMembre,
                    libelle = i.BIEN_libelle,
                    noteMoyenne = (double)i.BIEN_noteMoyenne,
                    numero = i.BIEN_numero,
                    pays = i.BIEN_Pays,
                    photo = i.BIEN_photo,
                    rue = i.BIEN_rue,
                    tarifNettoyage = i.BIEN_tarifNettoyage,
                    tarifParNuit = i.BIEN_tarifParNuit,
                    ville = i.BIEN_ville,
                    nbPersonnesMax = i.BIEN_nbMaxPersonnes
                });
            }
            return mesBiens;
        }

        [HttpGet]
        [Route("SearchBiens")]
        public List<Bien> _SearchBiens(string paysOuVille, string dateDepart, string dateRetour, int nbPers)
        {
            if (paysOuVille == null)
            {
                paysOuVille = "";
            }
            DateTime dateDeparts = DateTime.ParseExact(dateDepart, "dd/MM/yyyy", null);
            DateTime dateRetours = DateTime.ParseExact(dateRetour, "dd/MM/yyyy", null);


            DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
            List<Bien> mesBiensTrouves = new List<Bien>();
            
            
            List<DAL.SearchBiensDispo2_Result2> biens = dbContext.SearchBiensDispo2(paysOuVille, dateDeparts, dateRetours, nbPers).ToList();
            foreach (var b in biens)
            {
                mesBiensTrouves.Add(new Bien { idBien = (int)b.idBien, idMembre = (int)b.idMembre, libelle = b.libelle, noteMoyenne = (double)b.noteMoyenne, numero = b.numero, pays = b.pays, photo = b.photo, rue = b.rue, tarifNettoyage = (int)b.tarifNettoyage, tarifParNuit = (int)b.tarifNuit, ville = b.ville, nbPersonnesMax = (int)b.nbPersMax });
            }
            return mesBiensTrouves;

        }


        [HttpPost]
        [Route("PostNewLocation")]
        public IHttpActionResult PostNewLocation(LocationBien nvLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ERREUR, le formulaire a ete mal rempli!!!");
            }
            else
            {
                DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
                dbContext.addNewLocation(nvLocation.idBien, nvLocation.idMembre, nvLocation.dateArrivee, nvLocation.dateDepart);
                return Ok();
            }
        }

        [HttpPost]
        [Route("BigSearchBien")]
        public List<Bien> BigSearchBien (Model_FormBigSearchBien mesBiens)
        {
            List<Bien> mesBiensList = new List<Bien>();
            DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
            List<DAL.searchBienWithOptions_Result> mbiens = new List<DAL.searchBienWithOptions_Result>();
            mbiens = dbContext.searchBienWithOptions(mesBiens.bbq, mesBiens.piscine, mesBiens.jacuzzi, mesBiens.sauna, 
                mesBiens.tv, mesBiens.teleDistribution, mesBiens.wifi, mesBiens.pingpong, mesBiens.tennis, mesBiens.transat, 
                mesBiens.cuisineEquipee, mesBiens.machineALaver, mesBiens.jardin, mesBiens.parking, mesBiens.salleDeBain, 
                mesBiens.toilette, mesBiens.chambre, mesBiens.tarifParNuit, mesBiens.paysOuVille, mesBiens.nbPersonnesMax).ToList();
            foreach(var i in mbiens)
            {
                mesBiensList.Add(new Bien { idBien = (int)i.idBien, idMembre = (int)i.inMembre, pays = i.pays, ville = i.ville, rue = i.rue, numero = i.num, tarifParNuit = (int)i.tarifNuit, tarifNettoyage = (int)i.tarifNett, libelle = i.libelle, noteMoyenne = (double)i.noteM, photo = i.photo, nbPersonnesMax = (int)i.nbPersMax, bbq = (bool)i.bbq, jacuzzi = (bool)i.jacuzzi, jardin = (bool)i.jardin, parking = (bool)i.parking, machineALaver = (bool)i.machineALaver, cuisineEquipee = (bool)i.cuisineEq, pingpong = (bool)i.pingpong, wifi = (bool)i.wifi, sauna = (bool)i.sauna, tv = (bool)i.tv, tennis = (bool)i.tennis, piscine = (bool)i.piscine, teleDistribution = (bool)i.teleDis, transat = (bool)i.transat, chambre = (int)i.chambre, cuisine = (int)i.cuisine, dressing = (int)i.dressing, salleAManger = (int)i.sam, salleDeBain = (int)i.sdb, salon = (int)i.salon, toilette = (int)i.toilette, veranda = (int)i.veranda });
            }
            return mesBiensList;
        }

        [HttpGet]
        [Route("GETDatesPasDispo")]
        public List<DateTime> RechercheDaiesPasDispo(int idBien)
        {
            DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
            List<DAL.RechercheDatePasDispo_Result> datesPasDispo = dbContext.RechercheDatePasDispo(idBien).ToList();

            List<DateTime> dates = new List<DateTime>();
            foreach (var d in datesPasDispo)
            {
                dates.Add((DateTime)d.dateOcc);
            }

            return dates;
        }

        [Route("PostCommentAndNote")]
        public IHttpActionResult PostCommentAndNote(commentaireEtNote commentEtNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ERREUR, le formulaire a ete mal rempli!!!");
            }
            else
            {
                DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
                dbContext.addCommentAndNote(commentEtNote.note, commentEtNote.idLocation, commentEtNote.libelle);
                return Ok();
            }
        }

        [Route("GetListComment")]
        [HttpGet]
        public List<listeCommentaireDuBien> GetListComment(int idBien)
        {
            DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
            List<DAL.GetFiveLastComment_Result> listeCommentDB = dbContext.GetFiveLastComment(idBien).ToList();

            List<listeCommentaireDuBien> listeComment = new List<listeCommentaireDuBien>();
            foreach (var c in listeCommentDB)
            {
                listeComment.Add(new listeCommentaireDuBien { idBien = (int)c.idBien, dateMessage = (DateTime)c.dateComment, idCommentaire = (int)c.idCommentaire, libelle = c.libelle });
            }


            return listeComment;
        }


        //[HttpGet]
        //[Route("BigSearchBien")]
        //public List<Model_FormBigSearchBien> _BigSearchBien(string paysOuVille, string bbq, string piscine, string jacuzzi, string sauna, string tv, string teleDistribution, string wifi, string pingPong, string tennis, string transat, string cuisineEquipee, string machineALaver)
        //{
        //    DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
        //    List<Bien> mesBiensTrouves = new List<Bien>();
        //    List<DAL.BigSearch_Result> biens = dbContext.BigSearch(paysOuVille, bbq, piscine, jacuzzi, sauna, tv, teleDistribution, wifi, pingPong,  tennis,  transat,  cuisineEquipee,  machineALaver).ToList();
        //    foreach (var b in biens)
        //    {
        //        mesBiensTrouves.Add(new Bien { idBien = (int)b.idBien, idMembre = (int)b.idMembre, libelle = b.libelle, noteMoyenne = (decimal)b.noteMoyenne, numero = b.numero, pays = b.pays, photo = b.photo, rue = b.rue, tarifNettoyage = (decimal)b.tarifNettoyage, tarifParNuit = (decimal)b.tarifNuit, ville = b.ville, nbPersonnesMax = (int)b.nbPersMax });
        //    }
        //    return mesBiensTrouves;
        //}

    }
}
