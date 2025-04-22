using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MobiliTreeApi.Domain;
using MobiliTreeApi.Repositories;

namespace MobiliTreeApi.Controllers
{
    [Route("sessions")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionsRepository _sessionsRepository;

        public SessionsController(ISessionsRepository sessionsRepository)
        {
            _sessionsRepository = sessionsRepository;
        }

        [HttpPost]
        public ActionResult Post(Session value)
        {
            if(ModelState.IsValid )
            {
                _sessionsRepository.AddSession(value);
                return Ok();               
            }
            return BadRequest(ModelState);

        }

        [HttpGet]
        [Route("{parkingFacilityId}")]
        public ActionResult<List<Session>> Get(string parkingFacilityId)
        {
            return Ok(_sessionsRepository.GetSessions(parkingFacilityId));
        }
    }
}
