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
        private readonly ISaveEntityRepository<User> _saveEntityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IScopedService<User> _userScopedService;
        private readonly IMapper _mapper;

        public UsersController(
            ISaveEntityRepository<User> saveEntityRepository,
            IUserRepository userRepository,
            IScopedService<User> userScopedService,
            IMapper mapper)
        {
            _saveEntityRepository = saveEntityRepository;
            _userRepository = userRepository;
            _userScopedService = userScopedService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name="UserLink")]
        [UserNotFound("id")]
        public IActionResult Get(string id)
        {
            return Ok(_userScopedService.Get());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserCreateDto userCreateDto)
        {
            if (userCreateDto.Id == null || userCreateDto.Email == null)
                return BadRequest("id and email cannot be blank");

            var existingUser = await _userRepository.Get(userCreateDto.Id);

            if (existingUser != null)
                return Conflict("User with this id already exists");

            var user = _mapper.Map<User>(userCreateDto);

            user = await _saveEntityRepository.Save(user);

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
            var user = _userScopedService.Get();

            if (user == null) return NotFound("User not found");

            await _saveEntityRepository.Delete(user);

            return NoContent();
        }
    }
}
