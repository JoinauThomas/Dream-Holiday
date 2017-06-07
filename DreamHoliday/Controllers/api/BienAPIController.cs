using DreamHoliday.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DreamHoliday.DAL;

namespace DreamHoliday.Controllers
{
    [RoutePrefix("api/BienAPI")]

    public class BienAPIController : ApiController
    {

        // ajouter un nouveau bien

        [HttpPost]
        [Authorize]
        [Route("PostNewBien")]
        public int PostNewBien(Bien nvBien)
        {
            int idNvBien = 0;
            if (!ModelState.IsValid)
            {
                return idNvBien;
            }
            else
            {
                DreamHollidayEntities dbContext = new DreamHollidayEntities();
                List<DAL.addNewBiens_Result> monNvBienDB = new List< addNewBiens_Result>();
                monNvBienDB = dbContext.addNewBiens(nvBien.pays, nvBien.ville, nvBien.rue, nvBien.numero, nvBien.idMembre, nvBien.tarifParNuit,
                    nvBien.tarifNettoyage, nvBien.libelle, nvBien.nbPersonnesMax, nvBien.salleDeBain, nvBien.salon,
                    nvBien.salleAManger, nvBien.toilette, nvBien.cuisine, nvBien.chambre, nvBien.dressing, nvBien.veranda,
                    nvBien.bbq, nvBien.piscine, nvBien.jacuzzi, nvBien.sauna, nvBien.tv, nvBien.teleDistribution, nvBien.wifi,
                    nvBien.pingpong, nvBien.tennis, nvBien.transat, nvBien.cuisineEquipee, nvBien.machineALaver, nvBien.jardin, nvBien.parking).ToList();
                foreach(var x in monNvBienDB)
                {
                    idNvBien = x.idBien;
                }
                return idNvBien;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("EditBien")]
        public IHttpActionResult EditBien(Bien monBien)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ERREUR dans le formulaire");
            }
            else
            {
                DreamHollidayEntities dbContext = new DreamHollidayEntities();
                dbContext.editMyBien(monBien.idBien, monBien.pays, monBien.ville, monBien.rue, monBien.numero, monBien.tarifParNuit,
                    monBien.tarifNettoyage, monBien.libelle, monBien.photo, monBien.nbPersonnesMax, monBien.salleDeBain, monBien.salon,
                    monBien.salleAManger, monBien.toilette, monBien.cuisine, monBien.chambre, monBien.dressing, monBien.veranda,
                    monBien.bbq, monBien.piscine, monBien.jacuzzi, monBien.sauna, monBien.tv, monBien.teleDistribution, monBien.wifi,
                    monBien.pingpong, monBien.tennis, monBien.transat, monBien.cuisineEquipee, monBien.machineALaver, monBien.jardin, monBien.parking);
                return Ok();
            }
        }

        [HttpGet]
        [Authorize]
        [Route("DeleteBien")]
        public List<Bien> DeleteBien(int idBien)
        {
            List<Bien> mesBiens = new List<Bien>();

            DAL.DreamHollidayEntities dbContext = new DreamHollidayEntities();

            // recuperation d'idMembre
            List<BIEN> BiensListe = dbContext.BIEN.ToList();
            int idMembre = BiensListe.Find(x => x.idBien == idBien).idMembre;

            // suppression du bien
            dbContext.DeleteBien(idBien);

            // recuperation des biens du user
            List<DAL.GetAllMyBiens_Result> mesBiensDB = dbContext.GetAllMyBiens(idMembre).ToList();
            foreach (var b in mesBiensDB)
            {
                mesBiens.Add(new Bien
                {
                    idBien = b.idBien,
                    idMembre = b.idMembre,
                    libelle = b.BIEN_libelle,
                    noteMoyenne = Math.Round((double)b.BIEN_noteMoyenne, 2),
                    numero = b.BIEN_numero,
                    pays = b.BIEN_Pays,
                    photo = b.BIEN_photo,
                    rue = b.BIEN_rue,
                    tarifNettoyage = b.BIEN_tarifNettoyage,
                    tarifParNuit = b.BIEN_tarifParNuit,
                    ville = b.BIEN_ville,
                    nbPersonnesMax = b.BIEN_nbMaxPersonnes
                });
            }
            return mesBiens;
        }

        [HttpGet]
        [Route("GetAllBiens")]
        public List<Bien> GetAllBiens()
        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            List<BIEN> BiensListe = dbContext.BIEN.ToList();
            List<Bien> mesBiens = new List<Bien>();


            //recherche de l'id du nouveau bien

            foreach (var i in BiensListe)
            {
                mesBiens.Add(new Bien
                {
                    idBien = i.idBien,
                    idMembre = i.idMembre,
                    libelle = i.BIEN_libelle,
                    noteMoyenne = Math.Round((double)i.BIEN_noteMoyenne, 2),
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


            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            List<Bien> mesBiensTrouves = new List<Bien>();


            List<SearchBiensDispo_Result> biens = dbContext.SearchBiensDispo(paysOuVille, dateDeparts, dateRetours, nbPers).ToList();
            foreach (var b in biens)
            {
                mesBiensTrouves.Add(new Bien { idBien = (int)b.idBien, idMembre = (int)b.idMembre, libelle = b.libelle, noteMoyenne = (double)b.noteMoyenne, numero = b.numero, pays = b.pays, photo = b.photo, rue = b.rue, tarifNettoyage = (int)b.tarifNettoyage, tarifParNuit = (int)b.tarifNuit, ville = b.ville, nbPersonnesMax = (int)b.nbPersMax });
            }
            return mesBiensTrouves;

        }


        [HttpPost]
        [Authorize]
        [Route("PostNewLocation")]
        public IHttpActionResult PostNewLocation(LocationBien nvLocation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ERREUR, le formulaire a ete mal rempli!!!");
            }
            else
            {
                DreamHollidayEntities dbContext = new DreamHollidayEntities();
                dbContext.addNewLocation(nvLocation.idBien, nvLocation.idMembre, nvLocation.dateArrivee, nvLocation.dateDepart);
                return Ok();
            }
        }

        [HttpPost]
        [Route("BigSearchBien")]
        public List<Bien> BigSearchBien(Model_FormBigSearchBien mesBiens)
        {
            List<Bien> mesBiensList = new List<Bien>();
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            List<BigSearchBiens_Result> mbiens = new List<BigSearchBiens_Result>();
            mbiens = dbContext.BigSearchBiens(mesBiens.bbq, mesBiens.piscine, mesBiens.jacuzzi, mesBiens.sauna,
                mesBiens.tv, mesBiens.teleDistribution, mesBiens.wifi, mesBiens.pingpong, mesBiens.tennis, mesBiens.transat,
                mesBiens.cuisineEquipee, mesBiens.machineALaver, mesBiens.jardin, mesBiens.parking, mesBiens.salleDeBain,
                mesBiens.toilette, mesBiens.chambre, mesBiens.tarifParNuit, mesBiens.paysOuVille, mesBiens.nbPersonnesMax, (int)mesBiens.noteMoyenne).ToList();
            foreach (var i in mbiens)
            {
                mesBiensList.Add(new Bien { idBien = (int)i.idBien, idMembre = (int)i.inMembre, pays = i.pays, ville = i.ville, rue = i.rue, numero = i.num, tarifParNuit = (int)i.tarifNuit, tarifNettoyage = (int)i.tarifNett, libelle = i.libelle, noteMoyenne = (double)i.noteM, photo = i.photo, nbPersonnesMax = (int)i.nbPersMax, bbq = (bool)i.bbq, jacuzzi = (bool)i.jacuzzi, jardin = (bool)i.jardin, parking = (bool)i.parking, machineALaver = (bool)i.machineALaver, cuisineEquipee = (bool)i.cuisineEq, pingpong = (bool)i.pingpong, wifi = (bool)i.wifi, sauna = (bool)i.sauna, tv = (bool)i.tv, tennis = (bool)i.tennis, piscine = (bool)i.piscine, teleDistribution = (bool)i.teleDis, transat = (bool)i.transat, chambre = (int)i.chambre, cuisine = (int)i.cuisine, dressing = (int)i.dressing, salleAManger = (int)i.sam, salleDeBain = (int)i.sdb, salon = (int)i.salon, toilette = (int)i.toilette, veranda = (int)i.veranda });
            }
            return mesBiensList;
        }

        [HttpGet]
        [Route("GETDatesPasDispo")]
        public List<DateTime> RechercheDaiesPasDispo(int idBien)
        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            List<RechercheDatePasDispo_Result> datesPasDispo = dbContext.RechercheDatePasDispo(idBien).ToList();

            List<DateTime> dates = new List<DateTime>();
            foreach (var d in datesPasDispo)
            {
                dates.Add((DateTime)d.dateOcc);
            }

            return dates;
        }

        [HttpPost]
        [Authorize]
        [Route("PostCommentAndNote")]
        public IHttpActionResult PostCommentAndNote(commentaireEtNote commentEtNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("ERREUR, le formulaire a ete mal rempli!!!");
            }
            else
            {
                DreamHollidayEntities dbContext = new DreamHollidayEntities();
                dbContext.addCommentAndNote(commentEtNote.note, commentEtNote.idLocation, commentEtNote.libelle);
                return Ok();
            }
        }

        [HttpGet]
        [Route("GetListComment")]
        public List<listeCommentaireDuBien> GetListComment(int idBien)
        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            List<GetFiveLastComment_Result> listeCommentDB = dbContext.GetFiveLastComment(idBien).ToList();

            List<listeCommentaireDuBien> listeComment = new List<listeCommentaireDuBien>();
            foreach (var c in listeCommentDB)
            {
                listeComment.Add(new listeCommentaireDuBien { idBien = (int)c.idBien, dateMessage = (DateTime)c.dateComment, idCommentaire = (int)c.idCommentaire, libelle = c.libelle });
            }


            return listeComment;
        }

        [HttpGet]
        [Route("GetVerifDateDuBien")]
        public bool VerifDateDuBien(int idBien, DateTime dateArrivees, DateTime dateDeparts)
        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            List<verifDates2_Result> verif = dbContext.verifDates2(idBien, dateArrivees, dateDeparts).ToList();

            if (verif.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        [HttpGet]
        [Route("GetBienWithId")]
        public Bien GetBienWithId(int idBien)
        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            List<detailBienWithIde_Result> bienDB = dbContext.detailBienWithIde(idBien).ToList();

            Bien monBien = new Bien();

            foreach (var b in bienDB)
            {
                monBien = new Bien
                {
                    bbq = b.bbq,
                    chambre = b.chambre,
                    cuisine = b.cuisine,
                    cuisineEquipee = b.cuisine_equipee,
                    dressing = b.dressing,
                    idBien = b.idBien,
                    idMembre = b.idMembre,
                    jacuzzi = b.jacuzzi,
                    jardin = b.jardin,
                    libelle = b.BIEN_libelle,
                    machineALaver = b.machine_a_laver,
                    nbPersonnesMax = b.BIEN_nbMaxPersonnes,
                    noteMoyenne = (double)b.BIEN_noteMoyenne,
                    numero = b.BIEN_numero,
                    parking = b.parking,
                    pays = b.BIEN_Pays,
                    photo = b.BIEN_photo,
                    pingpong = b.pingpong,
                    piscine = b.piscine,
                    rue = b.BIEN_rue,
                    salleAManger = b.sallaAManger,
                    salleDeBain = b.salleDeBain,
                    salon = b.salon,
                    sauna = b.sauna,
                    tarifNettoyage = b.BIEN_tarifNettoyage,
                    tarifParNuit = b.BIEN_tarifParNuit,
                    teleDistribution = b.teledistribution,
                    tennis = b.tennis,
                    toilette = b.toilette,
                    transat = b.transat,
                    tv = b.tv,
                    veranda = b.veranda,
                    ville = b.BIEN_ville,
                    wifi = b.wifi
                };
            }


            return monBien;
        }

        [HttpGet]
        [Authorize]
        [Route("GetCountOfMyLocations")]
        public int GetCountOfMyLocations(int idMembre)
        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();

            List<LOCATION> listeLoc = dbContext.LOCATION.ToList();
            List<LOCATION> MesLocs = dbContext.LOCATION.ToList();

            MesLocs = listeLoc.FindAll(m => m.idMembre == idMembre);
            int nbLoc = MesLocs.Count();

            return nbLoc;

        }

        [HttpGet]
        [Authorize]
        [Route("GetCountOfMyMessages")]
        public int GetCountOfMyMessages(int idMembre)

        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            List<MESSAGE> mesMess = dbContext.MESSAGE.ToList().FindAll(m => m.MES_MEM_idMembre == idMembre);
            int nbMessage = mesMess.Count;

            return nbMessage;
        }

        [HttpGet]
        [Authorize]
        [Route("GetNbreOfNoteForBien")]
        public int GetNbreOfNoteForBien(int idBien)
        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            int nb = dbContext.NOTE.ToList().FindAll(n => n.idBien == idBien).Count;

            return nb;
        }


        [HttpGet]
        [Authorize]
        [Route("GetMyMessages")]
        public List<Message> GetMyMessages(int idMembre)
        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            List<MESSAGE> tousLesMessages = dbContext.MESSAGE.ToList().FindAll(x => x.MES_MEM_idMembre == idMembre);
            List<Message> mesMessages = new List<Message>();
            foreach (var m in tousLesMessages)
            {
                mesMessages.Add(new Message { idMembre = m.MES_MEM_idMembre, idMessage = m.idMessage, libelle = m.MES_message, mail = m.MES_mail, nom = m.MES_nom, prenom = m.MES_prenom, dateEnvoi = m.MES_date, sujet = m.MES_Sujet });
            }

            return mesMessages;
        }

        [HttpGet]
        [Authorize]
        [Route("GetDetailOfMyMessage")]
        public Message GetDetailOfMyMessage(int idMessage)
        {
            DreamHollidayEntities dbContext = new DreamHollidayEntities();
            MESSAGE LeMessageDB = dbContext.MESSAGE.ToList().Find(x => x.idMessage == idMessage);
            Message monMessage = new Message
            {
                idMessage = LeMessageDB.idMessage,
                dateEnvoi = LeMessageDB.MES_date,
                idMembre = LeMessageDB.MES_MEM_idMembre,
                libelle = LeMessageDB.MES_message,
                mail = LeMessageDB.MES_mail,
                nom = LeMessageDB.MES_nom,
                prenom = LeMessageDB.MES_prenom,
                sujet = LeMessageDB.MES_Sujet
            };

            return monMessage;
        }

        [HttpGet]
        [Authorize]
        [Route("GetmesBiens")]
        public List<Bien> GetmesBiens(int idMembre)
        {
            DAL.DreamHollidayEntities dbContext = new DreamHollidayEntities();

            List<DAL.GetAllMyBiens_Result> mesBiensDB = dbContext.GetAllMyBiens(idMembre).ToList();
            List<Bien> mesBiens = new List<Bien>();

            foreach(var b in mesBiensDB)
            {
                mesBiens.Add(new Bien
                {
                    idBien = b.idBien,
                    idMembre = b.idMembre,
                    libelle = b.BIEN_libelle,
                    noteMoyenne = Math.Round((double)b.BIEN_noteMoyenne,2),
                    numero = b.BIEN_numero,
                    pays = b.BIEN_Pays,
                    photo = b.BIEN_photo,
                    rue = b.BIEN_rue,
                    tarifNettoyage = b.BIEN_tarifNettoyage,
                    tarifParNuit = b.BIEN_tarifParNuit,
                    ville = b.BIEN_ville,
                    nbPersonnesMax = b.BIEN_nbMaxPersonnes
                });
            }
            return mesBiens;

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
