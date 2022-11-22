using AutoMapper;
using DynamoDB.DAL.App.Models;
using Microsoft.AspNetCore.Mvc;
using SimplifyStreaming.API.App.Common.Controllers;
using SimplifyStreaming.API.App.Common.Filters;
using SimplifyStreaming.API.App.Common.Services;

namespace SimplifyStreaming.API.App.Titles
{
    [Route("/[controller]")]
    public class TitlesController : BaseController
    {
        private readonly ITitleService _titleService;
        private readonly IMapper _mapper;

        public TitlesController(ITitleService titleService, IMapper mapper)
        {
            _titleService = titleService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "TitleLink")]
        [TitleNotFound("id")]
        public async Task<IActionResult> Get(string id)
        {
            var title = await _titleService.GetTitle(id);
            return Ok(title);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationCheckFilter))]
        public async Task<IActionResult> Post([FromBody] TitleCreateDto titleCreateDto)
        {
            var title = _mapper.Map<Title>(titleCreateDto);
            title = await _titleService.Save(title);

            return CreatedAtRoute(
                routeName: "TitleLink",
                routeValues: new { id = title.Id },
                value: title
            );
        }

        [HttpDelete("{id}")]
        [TitleNotFound("id")]
        public async Task<IActionResult> Delete(string id)
        {
            await _titleService.Delete(id);

            return NoContent();
        }
    }
}
