using AutoMapper;
using DynamoDB.DAL.App.Models;
using Microsoft.AspNetCore.Mvc;
using SimplifyStreaming.API.App.Common.Controllers;
using SimplifyStreaming.API.App.Common.Filters;

namespace SimplifyStreaming.API.App.ServiceTitles
{
    [Route("/services/{serviceId}/titles")]
    public class ServiceTitlesController : BaseController
    {
        private readonly IServiceTitleService _serviceTitleService;

        public ServiceTitlesController(IServiceTitleService serviceTitleService)
        {
            _serviceTitleService = serviceTitleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTitles(Service serviceId)
        {
            var serviceTitles = await _serviceTitleService.GetTitles(serviceId);
            return Ok(serviceTitles);
        }

        [HttpGet("{titleId}", Name = "ServiceTitleLink")]
        [TitleNotFound("titleId")]
        public async Task<IActionResult> GetTitle(Service serviceId, string titleId)
        {
            var serviceTitle = await _serviceTitleService.GetTitle(serviceId, titleId);
            return Ok(serviceTitle);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ServiceTitle serviceTitle)
        {
            serviceTitle = await _serviceTitleService.Save(serviceTitle);

            return CreatedAtRoute(
                routeName: "ServiceTitleLink",
                routeValues: new { serviceId = serviceTitle.ServiceId, titleId = serviceTitle.TitleId },
                value: serviceTitle
            );
        }

        [HttpDelete("{titleId}")]
        [TitleNotFound("titleId")]
        public async Task<IActionResult> Delete(Service serviceId, string titleId)
        {
            await _serviceTitleService.Delete(serviceId, titleId);
            return NoContent();
        }
    }
}
