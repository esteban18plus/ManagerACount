
#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ManagerACount.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ManagerACount.Data;
using ManagerACount.Data.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ManagerACount.Utilities;

#endregion

namespace ManagerACount.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {

        protected readonly EfManagerAccountContext _context;

        public AccountController(EfManagerAccountContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody]AccountDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dto.UserPassword = dto.UserPassword.Base64Encode();

                    var result = _context.Account.Where(a => a.UserEmail.ToLower() == dto.UserEmail.ToLower() && a.UserPassword == dto.UserPassword.ToLower()).FirstOrDefault();

                    if (result != null)
                    {
                        return BuildToken(dto);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error try",ex.Message);
                return BadRequest(ModelState);
            }
        }

        private IActionResult BuildToken(AccountDTO dto)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName,dto.UserEmail),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TG9yZW0gaXBzdW0gZG9sb3Igc2l0IGFtZXQsIGNvbnNlY3RldHVlciBhZGlwaXNjaW5nIGVsaXQuIEFlbmVhbiBjb21tb2RvIGxpZ3VsYSBlZ2V0IGRvbG9yLiBBZW5lYW4gbWFzc2EuIEN1bSBzb2NpaXMgbmF0b3F1ZSBwZW5hdGlidXMgZXQgbWFnbmlzIGRpcyBwYXJ0dXJpZW50IG1vbnRlcywgbmFzY2V0dXIgcmlkaWN1bHVzIG11cy4gRG9uZWMgcXVhbSBmZWxpcywgdWx0cmljaWVzIG5lYywgcGVsbGVudGVzcXVlIGV1LCBwcmV0aXVtIHF1aXMsIHNlbS4gTnVsbGEgY29uc2VxdWF0IG1hc3NhIHF1aXMgZW5pbS4gRG9uZWMgcGVkZSBqdXN0bywgZnJpbmdpbGxhIHZlbCwgYWxpcXVldCBuZWMsIHZ1bHB1dGF0ZSBlZ2V0LCBhcmN1LiBJbiBlbmltIGp1c3RvLCByaG9uY3VzIHV0LCBpbXBlcmRpZXQgYSwgdmVuZW5hdGlzIHZpdGFlLCBqdXN0by4gTnVsbGFtIGRpY3R1bSBmZWxpcyBldSBwZWRlIG1vbGxpcyBwcmV0aXVtLiBJbnRlZ2VyIHRpbmNpZHVudC4gQ3JhcyBkYXBpYnVzLiBWaXZhbXVzIGVsZW1lbnR1bSBzZW1wZXIgbmlzaS4gQWVuZWFuIHZ1bHB1dGF0ZSBlbGVpZmVuZCB0ZWxsdXMuIEFlbmVhbiBsZW8gbGlndWxhLCBwb3J0dGl0b3IgZXUsIGNvbnNlcXVhdCB2aXRhZSwgZWxlaWZlbmQgYWMsIGVuaW0uIEFsaXF1YW0gbG9yZW0gYW50ZSwgZGFwaWJ1cyBpbiwgdml2ZXJyYSBxdWlzLCBmZXVnaWF0IGEsIHRlbGx1cy4gUGhhc2VsbHVzIHZpdmVycmEgbnVsbGEgdXQgbWV0dXMgdmFyaXVzIGxhb3JlZXQuIFF1aXNxdWUgcnV0cnVtLiBBZW5lYW4gaW1wZXJkaWV0LiBFdGlhbSB1bHRyaWNpZXMgbmlzaSB2ZWwgYXVndWUuIEN1cmFiaXR1ciB1bGxhbWNvcnBlciB1bHRyaWNpZXMgbmlzaS4gTmFtIGVnZXQgZHVpLiBFdGlhbSByaG9uY3VzLiBNYWVjZW5hcyB0ZW1wdXMsIHRlbGx1cyBlZ2V0IGNvbmRpbWVudHVtIHJob25jdXMsIHNlbSBxdWFtIHNlbXBlciBsaWJlcm8sIHNpdCBhbWV0IGFkaXBpc2Npbmcgc2VtIG5lcXVlIHNlZCBpcHN1bS4gTmFtIHF1YW0gbnVuYywgYmxhbmRpdCB2ZWwsIGx1Y3R1cyBwdWx2aW5hciwgaGVuZHJlcml0IGlkLCBsb3JlbS4gTWFlY2VuYXMgbmVjIG9kaW8gZXQgYW50ZSB0aW5jaWR1bnQgdGVtcHVzLiBEb25lYyB2aXRhZSBzYXBpZW4gdXQgbGliZXJvIHZlbmVuYXRpcyBmYXVjaWJ1cy4gTnVsbGFtIHF1aXMgYW50ZS4gRXRpYW0gc2l0IGFtZXQgb3JjaSBlZ2V0IGVyb3MgZmF1Y2lidXMgdGluY2lkdW50LiBEdWlzIGxlby4gU2VkIGZyaW5naWxsYSBtYXVyaXMgc2l0IGFtZXQgbmliaC4gRG9uZWMgc29kYWxlcyBzYWdpdHRpcyBtYWduYS4gU2VkIGNvbnNlcXVhdCwgbGVvIGVnZXQgYmliZW5kdW0gc29kYWxlcywgYXVndWUgdmVsaXQgY3Vyc3VzIG51bmMs"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "esteban@gmail.com",
                audience: "esteban@gmail.com",
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expire = expiration,
            });
        }

        [HttpPost]
        [Route("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody]AccountDTO dto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = _context.Account.Where(a=> a.UserEmail.ToLower() == dto.UserEmail.ToLower()).FirstOrDefault();

                    if (result == null)
                    {
                        Mapper.Initialize(cfg => cfg.CreateMap<AccountDTO, Account>());
                        var entity = Mapper.Map<AccountDTO, Account>(dto);

                        var userEntity = new User()
                        {
                            CreationDate = DateTime.Now,
                        };

                        _context.User.Add(userEntity);
                        await _context.SaveChangesAsync();

                        entity.RoleId = 1;
                        entity.StateId = 1;
                        entity.UserId = userEntity.UserId;
                        entity.CreationDate = DateTime.Now;
                        entity.UserPassword = entity.UserPassword.Base64Encode();
                        _context.Account.Add(entity);
                        await _context.SaveChangesAsync();

                        entity.UserPassword = dto.UserPassword;

                        return Ok(new
                        {
                            Success = true,
                            Result = entity
                        });
                    }
                    else
                    {
                        ModelState.AddModelError("Error","El Correo eletrónico ya se encuentrá registrado.");
                        return BadRequest(ModelState);
                    }

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error try", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}