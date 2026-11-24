using ClinicAppointmentAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MedicalSessionsController : ControllerBase
	{
		private static List<MedicalSession> _sessions = new List<MedicalSession>
		{
			new MedicalSession
			{
				Id = 1,
				Title = "Kardioloji Müayinə",
				Description = "Dr. Rəşad Əliyev - EKQ və ümumi müayinə",
				RoomNumber = "Otaq 102",
				SessionDate = DateTime.Now.AddDays(1),
				MaxPatients = 10
			},
			new MedicalSession
			{
				Id = 2,
				Title = "Stomatoloji Baxış",
				Description = "Dr. Aysel Məmmədova - İlkin diaqnostika",
				RoomNumber = "Otaq 205",
				SessionDate = DateTime.Now.AddDays(2),
				MaxPatients = 8
			}
		};

		// GET: api/medicalsessions
		[HttpGet]
		public ActionResult<IEnumerable<MedicalSession>> GetAll()
		{
			return Ok(_sessions);
		}

		// GET: api/medicalsessions/1
		[HttpGet("{id}")]
		public ActionResult<MedicalSession> GetById(int id)
		{
			var session = _sessions.FirstOrDefault(s => s.Id == id);
			if (session == null)
			{
				return NotFound(new { message = $"ID-si {id} olan müayinə sessiyası tapılmadı." });
			}
			return Ok(session);
		}

		// POST: api/medicalsessions
		[HttpPost]
		public ActionResult<MedicalSession> Create([FromBody] MedicalSession newSession)
		{
			// Biznes məntiqi yoxlaması
			var validationError = ValidateSession(newSession);
			if (validationError != null)
			{
				return BadRequest(new { message = validationError });
			}

			newSession.Id = _sessions.Any() ? _sessions.Max(s => s.Id) + 1 : 1;
			_sessions.Add(newSession);

			// 201 Created qaytarır
			return CreatedAtAction(nameof(GetById), new { id = newSession.Id }, newSession);
		}

		// PUT: api/medicalsessions/1
		[HttpPut("{id}")]
		public IActionResult Update(int id, [FromBody] MedicalSession updatedSession)
		{
			var existingSession = _sessions.FirstOrDefault(s => s.Id == id);
			if (existingSession == null)
			{
				return NotFound(new { message = $"ID-si {id} olan müayinə sessiyası tapılmadı." });
			}

			// Yeniləmə zamanı da biznes qaydalarını yoxlayırıq
			var validationError = ValidateSession(updatedSession);
			if (validationError != null)
			{
				return BadRequest(new { message = validationError });
			}

			existingSession.Title = updatedSession.Title;
			existingSession.Description = updatedSession.Description;
			existingSession.RoomNumber = updatedSession.RoomNumber;
			existingSession.SessionDate = updatedSession.SessionDate;
			existingSession.MaxPatients = updatedSession.MaxPatients;

			// 204 No Content qaytarır
			return NoContent();
		}

		// DELETE: api/medicalsessions/1
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var session = _sessions.FirstOrDefault(s => s.Id == id);
			if (session == null)
			{
				return NotFound(new { message = $"ID-si {id} olan müayinə sessiyası tapılmadı." });
			}

			_sessions.Remove(session);

			// 204 No Content qaytarır
			return NoContent();
		}

		// Biznes qaydalarını yoxlayan köməkçi metod (Həm POST, həm PUT üçün işləyir)
		private string? ValidateSession(MedicalSession session)
		{
			if (string.IsNullOrWhiteSpace(session.Title))
			{
				return "Müayinə sessiyasının adı boş ola bilməz.";
			}

			if (session.SessionDate <= DateTime.Now)
			{
				return "Müayinə tarixi keçmiş tarix və ya indiki an ola bilməz.";
			}

			if (session.MaxPatients < 1)
			{
				return "Maksimum pasiyent sayı 1-dən az ola bilməz.";
			}

			return null;
		}
	}
}