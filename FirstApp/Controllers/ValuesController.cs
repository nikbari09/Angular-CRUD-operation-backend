using FirstApp.DataContect;
using FirstApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MyDataContext _context;
        public ValuesController(MyDataContext context)
        {
            _context = context;
        }
        
        [HttpGet("getEmployee")]
        public ActionResult<List<Employees>> GetYourEntities()
        {
            return _context.Employees.ToList();
        }

        [HttpGet("getEmployeebyuid/{id}")]
        public ActionResult<Employees> GetEmployeebyid(int id)
        {
            var emp = _context.Employees.Find(id);
            if(emp != null)
            {
                return emp;
            }
            return NotFound("Employee not found.");
        }

        [HttpPost]
        public ActionResult<String> PostData(Employees newEmp)
        {
            if (newEmp != null)
            {
                _context.Employees.Add(newEmp);
                _context.SaveChanges();
                return "Employee added..";
            }
            return "Emp not added";
        }
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult<String> Deleteemp(int id)
        {
            var emp = _context.Employees.Find(id);
            _context.Employees.Remove(emp);
            _context.SaveChanges();
            if(emp == null)
            {
                return "Not Found";
            }
            return "Deleted";
        }

        [HttpPut("updateEmployee/{id}")]
        public ActionResult<String> UpdateEmployee(int id,  Employees newEmp)
        {
            var emp =_context.Employees.Find(id);
            if(emp == null)
            {
                return NotFound("Employee not found.");
            }
            emp.firstname=newEmp.firstname;
            emp.lastname=newEmp.lastname;
            emp.email=newEmp.email;
            emp.dob=newEmp.dob;
            emp.gender = newEmp.gender;
            _context.SaveChanges();
            return "Employee updated sucessfully,";
        }
    }
}
