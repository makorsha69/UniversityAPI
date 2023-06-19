using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using UniversityAPI.Data_Context;
using UniversityAPI.Models;
using System.Text;

namespace UniversityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UniversityDBContext _context;

        public UsersController(UniversityDBContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.User == null)
          {
              return NotFound();
          }
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.User == null)
          {
              return Problem("Entity set 'UniversityDBContext.User'  is null.");
          }
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        #region GetUserbyEmail
        [HttpGet("GetUserbyEmail")]
        public async Task<ActionResult<User>> GetUserbyEmail(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(q => q.Email == email);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        #endregion


        #region ApproveUser
        [HttpGet("ApproveUser")]
        public async Task<ActionResult<User>> ApproveUser(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(q => q.Email == email);

            if (user != null && user.Access == false)
            {
                user.Access = true;
                user.Status = "Approved";
                await _context.SaveChangesAsync();
            }

            return Ok(user);
        }
        #endregion

        #region RejectUser
        [HttpGet("RejectUser")]
        public async Task<ActionResult<User>> RejectUser(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(q => q.Email == email);

            if (user != null)
            {
                user.Access = false;
                user.Status = "Rejected";
                await _context.SaveChangesAsync();
            }

            return Ok(user);
        }
        #endregion

        #region DeleteUser
        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.User == null)
            {
                return NotFound();
            }
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        #region Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(Login model)
        {
            var user = _context.User.FirstOrDefault(a => a.Email == model.Email);
            if (user != null && model.Password == user.Password)
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Name", user.Username.ToString())

                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YEg6R89Mlv21JbwO")), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
            {
                return BadRequest(new { message = "Email or Password is incorrect." });
            }
        }
        #endregion

        #region GetUserProfile

        [HttpGet("GetUserProfile")]
        [Authorize]

        public User GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserId").Value;
            User user = _context.User.Find(int.Parse(userId));
            return user;
        }

        #endregion

        #region EmailService
        [HttpGet("EmailService")]

        public IActionResult SendEmail(string name, string reciever)
        {
            string body = "Dear Sir/Ma'am, \n\n Hello " + name + ".Your email id " + reciever + " is succesfully registered with LOCOMOTIVE Railway Services \n\n Regards,\n Locomotive Railway Services Ltd.";
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("system.railwayinfo@gmail.com"));
            email.To.Add(MailboxAddress.Parse(reciever));
            email.Subject = "Registration comfirmation mail.";
            email.Body = new TextPart(TextFormat.Plain) { Text = body };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("system.railwayinfo@gmail.com ", "ruxidhbnmxoyoynz");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok("200");
        }
        #endregion

    }
}
