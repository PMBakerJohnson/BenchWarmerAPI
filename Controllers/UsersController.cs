using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BenchWarmerAPI.Models;
using Microsoft.AspNet.OData;

namespace BenchWarmerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly BenchwarmersContext _context;

        public UsersController(BenchwarmersContext context)
        {
            _context = context;
        }

        // POST: /login
        [HttpPost("/login")]
        public int Login([FromBody] Users userInfo)
        {
            if (UsernameExists(userInfo.Username))
            {
                Users user = _context.Users.FirstOrDefault(u => u.Username == userInfo.Username
                                                            && u.Upassword == userInfo.Upassword);
                if(user != null)
                {
                    return user.UserId;
                }
            }
            return 0;
        }

        // POST: /register
        [HttpPost("/register")]
        public int Register([FromBody] Users userInfo)
        {
            try
            {
                if(!UsernameExists(userInfo.Username))
                {

                    _context.Users.Add(userInfo);
                    _context.SaveChanges();
                    userInfo = _context.Users.FirstOrDefault(u => u.Username == userInfo.Username);

                    return userInfo.UserId;
                }
                else
                {
                    //using -1 as code for "username taken"
                    return -1;
                }
            }
            catch
            {
                //using 0 as code for error
                return 0;
            }
        }

        // GET: api/Users
        [HttpGet]
        [EnableQuery]
        public IEnumerable<Users> GetUsers()
        {
            return _context.Users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.Users.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers([FromRoute] int id, [FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != users.UserId)
            {
                return BadRequest();
            }

            _context.Entry(users).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException db)
            {
                if (!UsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    var exception = db.Message;
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody] Users users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Users.Add(users);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsers", new { id = users.UserId }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }

            _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            return Ok(users);
        }

        private bool UsernameExists(string username)
        {
            return _context.Users.Any(e => e.Username == username);
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}