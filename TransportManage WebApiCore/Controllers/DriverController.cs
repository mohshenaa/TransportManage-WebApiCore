using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManage_WebApiCore.Data;
using TransportManagementSystem.Models;

namespace TransportManage_WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 [Authorize]
   // [ApiExplorerSettings(IgnoreApi = true)]
    public class DriverController(TransportDb db) : ControllerBase
    {


        [HttpGet]
        public IQueryable<Driver> GetDrivers()
        {
            return db.Drivers;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            Driver driver = await db.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(int id, Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != driver.DriId)
            {
                return BadRequest();
            }

            db.Entry(driver).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drivers.Add(driver);
            await db.SaveChangesAsync();

            // return CreatedAtRoute("DefaultApi", new { id = driver.DriId }, driver);

            return Ok(driver);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return NotFound();
            }

            db.Drivers.Remove(driver);
            await db.SaveChangesAsync();

            return Ok(driver);
        }

     

        private bool DriverExists(int id)
        {
            return db.Drivers.Count(e => e.DriId == id) > 0;
        }

    //    [HttpPost("Upload/drivers")]
    //    public async Task<IActionResult> UploadDriverImage(
    //[FromServices] IImageUpload upload,
    //IFormFile file,
    //CancellationToken C)
    //    {
    //        var result = await upload.UploadFile(file, "drivers", C);
    //        return Ok(result);
    //    }
    }
}