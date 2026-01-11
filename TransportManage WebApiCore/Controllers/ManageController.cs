using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManage_WebApiCore.Data;



namespace TransportManage_WebApiCore.Controllers
{
	[Route("[controller]/[action]")]
	[ApiController]
//	[ApiExplorerSettings(IgnoreApi =true)]
	public class ManageController(IConfiguration configuration, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) : ControllerBase
	{


		[ActionName("Users"), HttpGet]
		public async Task<IActionResult> GetUser()
		{
			var data = await userManager.Users.ToListAsync();
			return Ok(data.Select(u => new { u.Id, u.UserName, u.Email, u.PhotoPath, u.PhoneNumber }));
		}

		[ActionName("Users"), HttpGet("{rolename}")]
		public async Task<IActionResult> GetUserByRole(string rolename)
		{
			var users = await userManager.GetUsersInRoleAsync(rolename);
			return Ok(users.Select(u => new { u.Id, u.UserName, u.Email, u.PhotoPath, u.PhoneNumber }));
		}


		[ActionName("Roles"), HttpGet]
		public async Task<IActionResult> GetRole()
		{
			var data = await roleManager.Roles.ToListAsync();

			return Ok(data.Select(r => new { r.Id, r.Name }));
		}

		[ActionName("Roles"), HttpGet("{userName}")]
		public async Task<IActionResult> GetRoleByUser(string userName)
		{
			var user= await userManager.FindByNameAsync(userName);
			if (user == null) {
				return BadRequest("Invalid User");
			}
			var roles= await userManager.GetRolesAsync(user);
			return Ok(roles);
		}


		[ActionName("SaveRoles"), HttpPost]
		public async Task<IActionResult> CreateRole(String roleName)
		{
			var result = await roleManager.CreateAsync(new IdentityRole(roleName));

			if (result.Succeeded)
			{
				return Ok($"{roleName} role created successfully");
			}


			return BadRequest(result.Errors);
		}

		[HttpPost]
		public async Task<IActionResult> AssignRole(UserRoleAssignDto data)
		{
			try
			{
				var user = await userManager.FindByNameAsync(data.UserName);
				if (user == null)
				{
					return BadRequest("Invalid user");
				}

				var oldRoles = await userManager.GetRolesAsync(user);
				await userManager.RemoveFromRolesAsync(user, oldRoles);
				await userManager.AddToRolesAsync(user, data.Roles);
			}
			catch (Exception er)
			{

				return BadRequest(er);
			}
			return Ok(data);
		}
	}
}
