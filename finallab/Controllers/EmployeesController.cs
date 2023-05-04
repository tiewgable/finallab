using finallab.Database;
using finallab.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace finallab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly DataDbContext _dbContext;

        public EmployeesController(DataDbContext DbContext)
        {
            _dbContext = DbContext;
        }
        //all get
        [HttpGet]
        public async Task<ActionResult<List<employees>>> getEmployees()
        {
            var employees = await _dbContext.employees.ToListAsync();
            if (employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        //get id
        [HttpGet("id")]
        public async Task<ActionResult<employees>> getEmployeeById(int id)
        {
            var employees = await _dbContext.employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        //Salary
        [HttpGet("current Salary ")]
        public async Task<ActionResult<employees>> getSalarycurrentYear(string id)
        {

            var employee = _dbContext.employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            var years = (DateTime.Now.Year - employee.hireDate.Year) - 1;

            var position = _dbContext.positions.Find(employee.positionId);
            if (position == null)
            {
                return NotFound();
            }
            var current = (position.baseSalary + (position.baseSalary * position.salaryIncreaseRate)) * years;

            return Ok(current);
        }
        //Post
        [HttpPost]
        public async Task<ActionResult<employees>> createEmployees(employees employee)
        {
            try
            {
                var position = _dbContext.positions.FirstOrDefault(p => p.positionId == employee.positionId);
                if (position == null)
                {
                    return BadRequest("Invalid position ID");
                }

                _dbContext.employees.Add(employee);
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok(employee);
        }
        //Put
        [HttpPut]
        public async Task<ActionResult<employees>> updateEmployees(int id, employees Newemployees)
        {
            try
            {
                if (_dbContext.positions.FirstOrDefault(p => p.positionId == Newemployees.positionId) == null)
                {
                    return BadRequest("Invalid position ID");
                }

                var employees = await _dbContext.employees.FindAsync(id);
                if (employees == null)
                {
                    return NotFound();
                }


                employees.empName = Newemployees.empName;
                employees.email = Newemployees.email;
                employees.phoneNumber = Newemployees.phoneNumber;
                employees.hireDate = Newemployees.hireDate;
                employees.positionId = Newemployees.positionId;

                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }




            return Ok(Newemployees);
        }
        //delete
        [HttpDelete]
        public async Task<ActionResult<employees>> deleteEmployees(string id)
        {
            var employees = await _dbContext.employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            _dbContext.employees.Remove(employees);

            await _dbContext.SaveChangesAsync();

            return Ok(employees);
        }

    }
}
