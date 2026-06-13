using Microsoft.AspNetCore.Mvc;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    // Note: Inject your IEnrollmentService here when ready as per Part A
    public EnrollmentsController() { }

    // GET /api/enrollments
    [HttpGet]
    public IActionResult GetAll()
    {
        // Returns 200 OK with an empty array to start
        return Ok(Array.Empty<object>()); 
    }
}
