using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2_WebAPI.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project2_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTelemetriesController : ControllerBase
    {
        private readonly masterContext _context;

        public JobTelemetriesController(masterContext context)
        {
            _context = context;
        }

        // GET: api/JobTelemetries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobTelemetry>>> GetJobTelemetries()
        {
            return await _context.JobTelemetries.ToListAsync();
        }

        // GET: api/JobTelemetries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JobTelemetry>> GetJobTelemetry(int id)
        {
            var jobTelemetry = await _context.JobTelemetries.FindAsync(id);

            if (jobTelemetry == null)
            {
                return NotFound();
            }

            return jobTelemetry;
        }

        // PUT: api/JobTelemetries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobTelemetry(int id, JobTelemetry jobTelemetry)
        {
            if (id != jobTelemetry.Id)
            {
                return BadRequest();
            }

            _context.Entry(jobTelemetry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobTelemetryExists(id))
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

        // POST: api/JobTelemetries
        [HttpPost]
        public async Task<ActionResult<JobTelemetry>> PostJobTelemetry(JobTelemetry jobTelemetry)
        {
            _context.JobTelemetries.Add(jobTelemetry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobTelemetry", new { id = jobTelemetry.Id }, jobTelemetry);
        }

        // DELETE: api/JobTelemetries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobTelemetry(int id)
        {
            var jobTelemetry = await _context.JobTelemetries.FindAsync(id);
            if (jobTelemetry == null)
            {
                return NotFound();
            }

            _context.JobTelemetries.Remove(jobTelemetry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Private method to check if a telemetry entry exists by ID
        private bool JobTelemetryExists(int id)
        {
            return _context.JobTelemetries.Any(e => e.Id == id);
        }

        // GET: api/JobTelemetries/GetSavingsByProject
        [HttpGet("GetSavingsByProject")]
        public async Task<ActionResult<IEnumerable<SavingsResult>>> GetSavingsByProject(int projectId, DateTime startDate, DateTime endDate)
        {
            var savings = await _context.JobTelemetries
                .Where(t => t.ProjectId == projectId && t.Timestamp >= startDate && t.Timestamp <= endDate)
                .GroupBy(t => t.ProjectId)
                .Select(g => new SavingsResult
                {
                    ProjectId = g.Key,
                    TotalTimeSaved = g.Sum(t => t.TimeSaved),
                    TotalCostSaved = g.Sum(t => t.CostSaved)
                }).ToListAsync();

            return Ok(savings);
        }

        // GET: api/JobTelemetries/GetSavingsByClient
        [HttpGet("GetSavingsByClient")]
        public async Task<ActionResult<IEnumerable<SavingsResult>>> GetSavingsByClient(int clientId, DateTime startDate, DateTime endDate)
        {
            var savings = await _context.JobTelemetries
                .Where(t => t.ClientId == clientId && t.Timestamp >= startDate && t.Timestamp <= endDate)
                .GroupBy(t => t.ClientId)
                .Select(g => new SavingsResult
                {
                    ClientId = g.Key,
                    TotalTimeSaved = g.Sum(t => t.TimeSaved),
                    TotalCostSaved = g.Sum(t => t.CostSaved)
                }).ToListAsync();

            return Ok(savings);
        }
    }

    // Define a model for returning the savings result
    public class SavingsResult
    {
        public int ProjectId { get; set; }
        public double TotalTimeSaved { get; set; }
        public double TotalCostSaved { get; set; }

        public int ClientId { get; set; }
    }
}

