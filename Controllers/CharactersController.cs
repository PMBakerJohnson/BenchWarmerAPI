using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BenchWarmerAPI.Models;
using Microsoft.AspNetCore.Cors;

namespace BenchWarmerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AngularLocalTestPolicy")]
    public class CharactersController : ControllerBase
    {
        private readonly BenchwarmersContext _context;

        public CharactersController(BenchwarmersContext context)
        {
            _context = context;
        }

        // GET: api/Characters
        [HttpGet]
        public IEnumerable<Characters> GetAllCharacters()
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
        [HttpGet("info/{characterID}")]
        public Characters GetCharacter([FromRoute] int characterID)
        {
            Characters character = _context.Characters.Find(characterID);
            character.ClassIdFkNavigation = _context.Classes.Find(character.ClassIdFk);
            return character;
        }

        // GET: api/Characters/PMBaker
        [HttpGet("{id}")]
        public IEnumerable<Characters> GetMyCharacters([FromRoute] int id)
        {
            var characters =  _context.Characters.Where(c => c.UserIdFk == id).Include("ClassIdFkNavigation");

            return characters;
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
        public bool PostCharacters([FromBody] Characters characters)
        {
            try
            {
                if (!_context.Classes.Any(c => c.ClassName == characters.ClassIdFkNavigation.ClassName))
                {
                    _context.Classes.Add(characters.ClassIdFkNavigation);
                    _context.SaveChanges();
                }

                Classes existing = _context.Classes.FirstOrDefault(c => c.ClassName == characters.ClassIdFkNavigation.ClassName);

                characters.ClassIdFk = existing.ClassId;

                _context.Characters.Add(characters);
                _context.SaveChanges();

                characters = _context.Characters.FirstOrDefault(c => c.FullName == characters.FullName);

                return true;
            }
            catch
            {
                return false;
            }
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