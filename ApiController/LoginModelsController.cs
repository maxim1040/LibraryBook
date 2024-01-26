using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryBook.ApiModels;
using LibraryBook.Areas.Data;

namespace LibraryBook.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginModelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LoginModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginModel>>> GetLoginModel()
        {
            return await _context.LoginModel.ToListAsync();
        }

        // GET: api/LoginModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoginModel>> GetLoginModel(int? id)
        {
            var loginModel = await _context.LoginModel.FindAsync(id);

            if (loginModel == null)
            {
                return NotFound();
            }

            return loginModel;
        }

        // PUT: api/LoginModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoginModel(int? id, LoginModel loginModel)
        {
            if (id != loginModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(loginModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoginModelExists(id))
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

        // POST: api/LoginModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoginModel>> PostLoginModel(LoginModel loginModel)
        {
            _context.LoginModel.Add(loginModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoginModel", new { id = loginModel.Id }, loginModel);
        }

        // DELETE: api/LoginModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoginModel(int? id)
        {
            var loginModel = await _context.LoginModel.FindAsync(id);
            if (loginModel == null)
            {
                return NotFound();
            }

            _context.LoginModel.Remove(loginModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoginModelExists(int? id)
        {
            return _context.LoginModel.Any(e => e.Id == id);
        }
    }
}
