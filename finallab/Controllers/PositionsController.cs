using finallab.Database;
using finallab.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace finallab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly DataDbContext _dbContext;

        //Cotructure Method
        public PositionsController(DataDbContext DbContext)
        {
            _dbContext = DbContext;
        }
        //all get
        [HttpGet]
        public async Task<ActionResult<List<positions>>> getPositions()
        {
            var position = await _dbContext.positions.ToListAsync();

            if (position.Count == 0)
            {
                return NotFound();
            }

            return Ok(position);
        }
        // get id
        [HttpGet("id")]
        public async Task<ActionResult<positions>> getPositionsID(string id)
        {
            var positions = await _dbContext.positions.FindAsync(id);
            if (positions == null)
            {
                return NotFound();
            }
            return Ok(positions);
        }
        //getPositions
        [HttpGet("Positions")]
        public async Task<ActionResult<positions>> getEmpPositionsID(string id)
        {
            var position = _dbContext.employees.FirstOrDefault(e => e.empId == id);
            if (position == null)
            {
                return NotFound();
            }
            return Ok(position);
        }
        //Post
        [HttpPost]
        public async Task<ActionResult<positions>> postPosition(positions position)
        {
            try
            {
                _dbContext.positions.Add(position);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok(position);
        }

        //Put
        [HttpPut]
        public async Task<ActionResult<positions>> putPosition(int id, positions Positions)
        {
            var positions = await _dbContext.positions.FindAsync(id);
            if (positions == null)
            {
                return NotFound();
            }

            positions.positionId = Positions.positionId;
            positions.positionName = Positions.positionName;
            positions.baseSalary = Positions.baseSalary;
            positions.salaryIncreaseRate = Positions.salaryIncreaseRate;

            await _dbContext.SaveChangesAsync();
            return Ok(positions);
        }

        //Delete
        [HttpDelete]
        public async Task<ActionResult<positions>> deletePositions(string id)
        {
            var employees = _dbContext.employees.Where(e => e.positionId == id).ToList();
            if (employees != null && employees.Count > 0)
            {
                return BadRequest("Cannot delete position with employees assigned to it.");
            }
            var position = _dbContext.positions.SingleOrDefault(p => p.positionId == id);
            if (position == null)
            {
                return NotFound();
            }
            _dbContext.positions.Remove(position);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}
