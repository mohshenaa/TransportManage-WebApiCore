using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TransportManage_WebApiCore.Data;

namespace TransportManage_WebApiCore.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]

	public class TokenController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration Configuration) : ControllerBase
	{
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register(UserDto UDto)
		{
			try
			{
				var appuser = new AppUser
				{
					UserName = UDto.UserName,
					Email = UDto.Email					
					,PhotoPath="/"
				};
				var result = await userManager.CreateAsync(appuser, UDto.Password);

				if (result.Succeeded)
				{
					return Ok(new { msg = "Regiatration Success" });
				}
				else
				{
					return BadRequest(result.Errors);
				}
			}
			catch (Exception er)
			{
				return BadRequest(er);
			}
         
        }
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginDto UDto)
		{
			try
			{
				var user = await userManager.FindByNameAsync(UDto.UserName);
				if (user == null)
				{
					return BadRequest(new { msg = "Bad credential" });
				}
				var success = await userManager.CheckPasswordAsync(user, UDto.Password);

				if (success)
				{

					var Claims = new List<Claim>();

					Claims.Add(new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()));

					Claims.Add(new Claim(ClaimTypes.Name, user.UserName));

					Claims.Add(new Claim(ClaimTypes.Email, user.Email));

					//foreach (var role in await userManager.GetRolesAsync(user))
					//{
					//	Claims.Add(new Claim(ClaimTypes.Role, role));
					//}

					Claims.AddRange((await userManager.GetRolesAsync(user)).Select (role=> new Claim(ClaimTypes.Role,role)));

					var Keybytes = Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:Key"));

					var securityKey= new SymmetricSecurityKey(Keybytes);	

					var signCredential = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

					var issuer = Configuration.GetValue<string>("Jwt.Issuer");

					var jwttoken = new JwtSecurityToken(claims: Claims, expires: DateTime.Now.AddDays(20),signingCredentials: signCredential);

					var token = new JwtSecurityTokenHandler().WriteToken(jwttoken);
					return Ok(new { token , name= user .UserName});
				}
				else
				{
					return BadRequest(new { Msg = "Bad credential" });
				}
			}
			catch (Exception ex)
			{

				return BadRequest(ex);
			}
			return Ok(UDto);
		}


		[Authorize, HttpGet]
		public async Task<IActionResult> GetuserInfo()
		{
			string name = "", email = "", roles = "";
			try
			{
				var loggedInUser = HttpContext.User;

				if (loggedInUser != null)
				{
					name = loggedInUser.Identity.Name;

					foreach (var claim in loggedInUser.Claims)
					{
						if (claim.Type == ClaimTypes.Email)
						{
							email = claim.Value;
						}
						else if (claim.Type == ClaimTypes.Role)
						{
							roles = claim.Value;
						}
					}
					
				}
			}
			catch
			{
				return BadRequest();
			}

			return Ok(new { name, email, roles=string.Join(",",roles) });
		}
	}



}
