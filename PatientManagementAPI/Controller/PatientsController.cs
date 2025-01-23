using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagementAPI.Data;
using PatientManagementAPI.Models;

namespace PatientManagementAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAllPatients()
        {
            return Ok(await _context.Patients.ToListAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            return patient == null ? NotFound() : Ok(patient);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPatient([FromBody] Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient patient)
        {
            var existingPatient = await _context.Patients.FindAsync(id);
            if (existingPatient == null) return NotFound();

            existingPatient.Name = patient.Name;
            existingPatient.Illness = patient.Illness;
            existingPatient.AdmissionDate = patient.AdmissionDate;
            existingPatient.RecheckUpDate = patient.RecheckUpDate;
            existingPatient.AdditionalDetails = patient.AdditionalDetails;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
