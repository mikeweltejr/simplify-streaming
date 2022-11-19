using AutoMapper;
using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SimplifyStreaming.API.App.Common.Controllers;
using SimplifyStreaming.API.App.Common.Filters;
using SimplifyStreaming.API.App.Common.Services;

namespace SimplifyStreaming.API.App.Users
{
    [Route("/controller")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name="UserLink")]
        [UserNotFound("id")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationCheckFilter))]
        public async Task<IActionResult> Post([FromBody] UserCreateDto userCreateDto)
        {
            var existingUser = await _userService.GetUser(userCreateDto.Id!);

            if (existingUser != null)
                return Conflict("User with this id already exists");

            var user = _mapper.Map<User>(userCreateDto);

            user = await _userService.Save(user);

            return CreatedAtRoute(
                routeName: "UserLink",
                routeValues: new { id = user.Id },
                value: user
            );
        }

        [HttpDelete("{id}")]
        [UserNotFound("id")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.Delete(id);

            return NoContent();
        }
    }
}
