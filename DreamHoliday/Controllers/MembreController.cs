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

namespace DreamHoliday.Controllers
{
    public class MembreController : BaseController
    {
        private static int idMembreBien;

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
            catch(Exception ex)
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
            return View();
        }

        [HttpPost]
        public ActionResult InsertNewMembre( string dateNaiss, Membre nouveauMembre, HttpPostedFileBase monfichier)
        {
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
                            //string accountName = "dreamholidayresource";
                            //string accessKey = "LPty8J8Q44CoRBXCCk/JLpd83gYrzQYXldG3wDrGn+lh5QVNelRbu44nY4Y5W2QsZ9NdH9TuCL3TdOIYifd3Rw==";
                            //string connectStr = "DefaultEndpointsProtocol=https;AccountName=dreamholidayresource;AccountKey=cJp91hGWZMvGQz4wi2MhyFtomq9T0v7RF2+gLkrZpkEB5SxbOoh8C0nK53mBncLHpxscP/vnVVggCUGWn+knsA==;EndpointSuffix=core.windows.net";



                            //StorageCredentials creden = new StorageCredentials(accountName, accessKey);
                            //CloudStorageAccount storageAccount = new CloudStorageAccount(creden, useHttps: false);
                            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                            //CloudBlobContainer container = blobClient.GetContainerReference("dreamholidaycontener");
                            //container.CreateIfNotExists();
                            //container.SetPermissions(
                            //    new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }
                            //    );
                            //CloudBlockBlob blockBlob = container.GetBlockBlobReference(monfichier.FileName);
                            //MemoryStream ms = new MemoryStream();
                            //Image img = Image.FromStream(monfichier.InputStream);
                            //img.Save(ms, ImageFormat.Jpeg);
                            //ms.Position = 0;
                            //blockBlob.UploadFromStream(ms);


                            //string sConn = "DefaultEndpointsProtocol=https;AccountName=dreamholidayresource;AccountKey=LPty8J8Q44CoRBXCCk/JLpd83gYrzQYXldG3wDrGn+lh5QVNelRbu44nY4Y5W2QsZ9NdH9TuCL3TdOIYifd3Rw==;EndpointSuffix=core.windows.net";
                            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectStr);
                            //string aa = "ee";
                            //CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                            //CloudBlobContainer blobContainer = blobClient.GetContainerReference("mycontainer");
                            //blobContainer.CreateIfNotExists();
                            //blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
                            //CloudBlockBlob blob = blobContainer.GetBlockBlobReference(monfichier.FileName);
                            //blob.Properties.ContentType = monfichier.ContentType;
                            //blob.UploadFromStream(monfichier.InputStream);

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
        public ActionResult updateMembre(editProfile moi, HttpPostedFileBase monfichier)
        {
            try
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


                    client.BaseAddress = new Uri("http://dreamholiday.azurewebsites.net/api/MembreAPI/");
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