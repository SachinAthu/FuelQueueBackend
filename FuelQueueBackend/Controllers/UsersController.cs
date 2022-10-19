using FuelQueueBackend.Models;
using FuelQueueBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuelQueueBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController: ControllerBase
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService;
        }

        // get all users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var res = await _usersService.GetAllUsers();
                return new JsonResult(res);
            } catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // get user by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var res = await _usersService.GetUserById(id);

                // no user found
                if(res == null) return NotFound();

                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // create/register user
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                var res = await _usersService.CreateUser(user);

                // user already exists
                if (res == 1) return StatusCode(403);

                return new CreatedAtActionResult(nameof(CreateUser), "Users", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // update user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] User user)
        {
            try
            {
                var res = await _usersService.UpdateUser(id, user);

                // no user found
                if (res == 1) return NotFound();

                if (res == 2) return StatusCode(403);

                return new CreatedAtActionResult(nameof(UpdateUser), "Users", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // delete user
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
                var res = await _usersService.DeleteUser(id);

                // no user found
                if (res == 1) return NotFound();

                return Ok(id);
            try
            {
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            try
            {
                var res = await _usersService.LoginUser(user);

                // invalid request
                if (res == "1") return BadRequest();

                // invalid credentials, no user found
                if (res == "2") return NotFound();

                return Ok(res);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

    }
}
