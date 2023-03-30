using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    //https://localhost:portno/api/students 
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentnames = new string[] { "uday", "mathur", "happy" };

            return Ok(studentnames);
        }
    }
}
