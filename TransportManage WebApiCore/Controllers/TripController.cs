using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManage_WebApiCore.Data;
using TransportManage_WebApiCore.Models;
using TransportManagementSystem.Models;

namespace TransportManage_WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TripController(TransportDb db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTrips()
        {

            var data = await db.Trips
                 .Include(t => t.Passengers)
                     .Include(t => t.Driver)
                     .Include(t => t.Vehicle)
                     .ToListAsync();

            return Ok(data);
        }

        [HttpGet("{id}")]
        public IActionResult GetTrip(int? id)
        {

            if (id is null)
                return BadRequest("ID is required");

            var data = db.Trips.Include(t => t.Passengers).Include(a => a.Vehicle).Include(a => a.Driver).FirstOrDefault(d => d.TripId == id);

            if (data == null)
                return NotFound($"Trip id {id} not found");

            return Ok(data);
        }



        [HttpPut("{id}")]
        public IActionResult PutTrip(int id, Trip updatedTrip)
        {
            if (id != updatedTrip.TripId)
                return BadRequest(new {result= "Id mismatch" });

            try
            {
                
                var existingTrip = db.Trips
                    .Include(t => t.Passengers)
                    .Include(t => t.Driver)
                    .Include(t => t.Vehicle)
                    .FirstOrDefault(t => t.TripId == id);

                if (existingTrip == null)
                    return NotFound($"Trip id {id} not found");

            
                existingTrip.StartDateTime = updatedTrip.StartDateTime;

                existingTrip.StartLocation = updatedTrip.StartLocation;
                existingTrip.Destination = updatedTrip.Destination;

                existingTrip.Helper = updatedTrip.Helper;
                existingTrip.EndDate = updatedTrip.EndDate;


                existingTrip.DistanceKm = updatedTrip.DistanceKm;

                existingTrip.Status = updatedTrip.Status;
               

                if (updatedTrip.DriverId > 0)
                {
                    existingTrip.DriverId = updatedTrip.DriverId;
                   
                    existingTrip.Driver = db.Drivers.Find(updatedTrip.DriverId);
                }

              
                if (updatedTrip.VehicleId > 0)
                {
                    existingTrip.VehicleId = updatedTrip.VehicleId;
                   
                    existingTrip.Vehicle = db.Vehicles.Find(updatedTrip.VehicleId);
                }           
                if (updatedTrip.Passengers != null)
                {
                   
                    var updatedPassengerIds = updatedTrip.Passengers
                        .Where(p => p.PsngrId > 0)
                        .Select(p => p.PsngrId)
                        .ToList();

                    var passengersToRemove = existingTrip.Passengers
                        .Where(p => p.PsngrId > 0 && !updatedPassengerIds.Contains(p.PsngrId))
                        .ToList();

                    foreach (var passenger in passengersToRemove)
                    {
                        existingTrip.Passengers.Remove(passenger);
                        db.Passengers.Remove(passenger);
                    }

                    foreach (var updatedPassenger in updatedTrip.Passengers)
                    {
                        if (updatedPassenger.PsngrId > 0)
                        {
                           
                            var existingPassenger = existingTrip.Passengers
                                .FirstOrDefault(p => p.PsngrId == updatedPassenger.PsngrId);

                            if (existingPassenger != null)
                            {
                                existingPassenger.PsngrName = updatedPassenger.PsngrName;
                                existingPassenger.PsngrContact = updatedPassenger.PsngrContact;
                                existingPassenger.Seatno = updatedPassenger.Seatno;
                                existingPassenger.ImageUrl = updatedPassenger.ImageUrl;
                            }
                        }
                        else
                        {
                           
                            existingTrip.Passengers.Add(new Passenger
                            {
                                PsngrName = updatedPassenger.PsngrName,
                                PsngrContact= updatedPassenger.PsngrContact,
                                Seatno = updatedPassenger.Seatno,
                                ImageUrl= updatedPassenger.ImageUrl,
                                TripId = existingTrip.TripId
                            });
                        }
                    }
                }
               
                db.SaveChanges();
                return Ok(existingTrip);
            }
            catch (DbUpdateException ex)
            {
               ;
				return BadRequest(new { result = $"Error updating trip: {ex.Message}" });
			}
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult PostTrip(Trip trip)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var lastTrip = db.Trips.OrderByDescending(t => t.TripId).FirstOrDefault();
            trip.TripId = lastTrip == null ? 1 : lastTrip.TripId + 1;


            foreach (var p in trip.Passengers)
            {
                p.PsngrId = 0;
            }

            db.Trips.Add(trip);
            db.SaveChanges();

            var createdTrip = db.Trips
                .Include(t => t.Driver)
                .Include(t => t.Vehicle)
                .Include(t => t.Passengers)
                .FirstOrDefault(t => t.TripId == trip.TripId);

            return Ok(createdTrip);
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteTrip(int id)
        {
            try
            {
                var trip = db.Trips.Find(id);
                if (trip == null)
                    return NotFound($"Trip id {id} not found");

                db.Trips.Remove(trip);
                db.SaveChanges();

                return Ok(new { result = $"Trip id {id} deleted successfully" });
            }
            catch (Exception ex)
            {
                //return BadRequest($"Error deleting trip: {ex.Message}");
				return BadRequest(new { result = $"Error deleting trip: {ex.Message}" });
			}
        }

        [HttpGet("{tripId}/passengers")]
        public async Task<IActionResult> GetPassengersByTrip(int tripId)
        {
            var passengers = await db.Passengers
                .Where(p => p.TripId == tripId)
                .ToListAsync();

            return Ok(passengers);
        }

        [HttpGet("passengers")]
        public async Task<IActionResult> GetPassengers()
        {
            var passengers = await db.Passengers.ToListAsync();

            return Ok(passengers);
        }

		[HttpPost("Upload/passengers")]
		public async Task<IActionResult> UploadPassengerImage(
	   [FromServices] IImageUpload upload,
	   [FromForm] UploadFileModel input,
	   CancellationToken C)
		{

			var result = await upload.UploadFile(input.File, "passengers", C);
			return Ok(result);
		}
	}
}
