using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MobiliTreeApi.Domain;
using MobiliTreeApi.Exceptions;
using MobiliTreeApi.Services;

namespace MobiliTreeApi.Controllers
{
    [Route("invoices")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        [Route("ping")]
        public string Get()
        {
            return "pong!";
        }

        [HttpGet]
        [Route("{parkingFacilityId}")]
        public ActionResult<List<Invoice>> Get(string parkingFacilityId)
        {
            var invoiceResult = _invoiceService.GetInvoices(parkingFacilityId);
            return invoiceResult.Match<ActionResult>(
                invoices =>
                {
                    return Ok(invoices);
                },
                exception =>
                {
                    if (exception is ParkingFacilityNotFoundException)
                    {
                        return NotFound(exception.Message);
                    }
                    return BadRequest(exception.Message);
                });
        }
    }
}
