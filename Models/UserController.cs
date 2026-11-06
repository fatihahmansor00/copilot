using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static readonly List<User> Users = new()
        {
            new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@techhive.com",
                Department = "IT"
            },
            new User
            {
                Id = 2,
                FirstName = "Sarah",
                LastName = "Lee",
                Email = "sarah.lee@techhive.com",
                Department = "HR"
            }
        };

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(Users);
        }

        // GET: api/users/1
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} was not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Error retrieving user: {ex.Message}");
            }
        }

        // POST: api/users
        [HttpPost]
    public ActionResult<User> CreateUser(User user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Users.Any(u =>
                u.Email.Equals(user.Email,
                StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Email already exists.");
            }

            int newId = Users.Any()
                ? Users.Max(u => u.Id) + 1
                : 1;

            user.Id = newId;

            Users.Add(user);

            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.Id },
                user);
        }
        catch (Exception ex)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                ex.Message);
        }
    }

        // PUT: api/users/1
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User updatedUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = Users.FirstOrDefault(u => u.Id == id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
                user.Department = updatedUser.Department;

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        // DELETE: api/users/1
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                Users.Remove(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}