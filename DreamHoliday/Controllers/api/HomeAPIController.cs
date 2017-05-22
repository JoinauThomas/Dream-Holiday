using DreamHoliday.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DreamHoliday.Controllers.api
{
    [RoutePrefix("api/HomeAPI")]
    public class HomeAPIController : ApiController
    {
        [HttpPost]
        [Route("PostQuestion")]
        public IHttpActionResult PostQuestion(question questionaire)
        {
            try
            {

                DAL.DreamHollidayEntities dbContext = new DAL.DreamHollidayEntities();
                dbContext.addNewMessage(questionaire.nom, questionaire.prenom, questionaire.mail, questionaire.message, 0, questionaire.sujet);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex+ "le message n'a pas pu etre envoyé");
            }

        }
    }
}
