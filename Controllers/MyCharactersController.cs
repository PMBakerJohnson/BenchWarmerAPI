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
    public class MyCharactersController : ControllerBase
    {
        private readonly BenchwarmersContext _context;

        public MyCharactersController(BenchwarmersContext context)
        {
            _context = context;
        }
        [HttpGet("{username}")]
        public IEnumerable<MyCharacters> GetCharactersByUserName([FromRoute] string username)
        {

            Users user = _context.Users.SingleOrDefault(e => e.Username == username);


            List<Characters> userCharacters = _context.Characters.ToList().FindAll(m => m.UserIdFk == user.UserId);

            //creates a list of player characters formatted for a table  without keys
            List<MyCharacters> playerCharacters = new List<MyCharacters>();

            foreach (Characters character in userCharacters)
            {
                MyCharacters characters = new MyCharacters();
                characters.Class = _context.Classes.ToList().Find(m => m.ClassId == character.ClassIdFk).ClassName;
                characters.CharacterName = character.FullName;
                playerCharacters.Add(characters);
            }

            return playerCharacters;

        }
    }
}