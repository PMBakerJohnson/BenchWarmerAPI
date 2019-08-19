using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BenchWarmerAPI.Models;

namespace BenchWarmerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly BenchwarmersContext _context;

        public CharactersController(BenchwarmersContext context)
        {
            _context = context;
        }

        // GET: api/Characters
        [HttpGet]
        public IEnumerable<Characters> GetCharacters()
        {
            return _context.Characters;
        }
        ////returns all the players characters it's in the My characters controller for tables
        //[HttpGet("{username}")]
        //public IEnumerable<MyCharacters> GetCharactersByUserName([FromRoute] string username)
        //{

        //    Users user = _context.Users.SingleOrDefault(e => e.Username == username);

            
        //   List<Characters> userCharacters= _context.Characters.ToList().FindAll(m => m.UserIdFk == user.UserId);

        //    //creates a list of player characters formatted for a table  without keys
        //   List<MyCharacters> playerCharacters = new List<MyCharacters>();

        //    foreach(Characters character in userCharacters)
        //    {
        //        MyCharacters characters = new MyCharacters();
        //        characters.Class= _context.Classes.ToList().Find(m => m.ClassId == character.ClassIdFk).ClassName;
        //        characters.CharacterName = character.FullName;
        //        playerCharacters.Add(characters);
        //    }

        //    return playerCharacters;

        //}
        // GET: api/Characters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacters([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var characters = await _context.Characters.FindAsync(id);

            if (characters == null)
            {
                return NotFound();
            }

            return Ok(characters);
        }

        // PUT: api/Characters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacters([FromRoute] int id, [FromBody] Characters characters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != characters.CharacterId)
            {
                return BadRequest();
            }

            _context.Entry(characters).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharactersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Characters
        [HttpPost]
        public async Task<IActionResult> PostCharacters([FromBody] Characters characters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Characters.Add(characters);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharacters", new { id = characters.CharacterId }, characters);
        }

        // DELETE: api/Characters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacters([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var characters = await _context.Characters.FindAsync(id);
            if (characters == null)
            {
                return NotFound();
            }

            _context.Characters.Remove(characters);
            await _context.SaveChangesAsync();

            return Ok(characters);
        }

        private bool CharactersExists(int id)
        {
            return _context.Characters.Any(e => e.CharacterId == id);
        }
    }
}