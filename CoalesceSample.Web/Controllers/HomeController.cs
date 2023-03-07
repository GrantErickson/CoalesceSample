using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoalesceSample.Web.Controllers;
public class HomeController : Controller
{
    /// <summary>
    /// Spa route for vue-based parts of the app
    /// </summary>
    // Prevent caching of this route.
    // The served file will contain the links to compiled js/css that include hashes in the filenames.
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Index(
        [FromServices] IWebHostEnvironment hostingEnvironment
    )
    {
        var fileInfo = hostingEnvironment.WebRootFileProvider.GetFileInfo("index.html");
        if (!fileInfo.Exists) return NotFound($"{hostingEnvironment.WebRootPath}/index.html was not found");

        return File(fileInfo.CreateReadStream(), "text/html");
    }

    public IActionResult Error()
    {
        ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return View();
    }

    [HttpGet("/test123")]
    public string Testx()
    {
        var test = User.Identities.ToList().ToString();

        return "Test method has run.";
    }
    [HttpGet("/anon")]
    [AllowAnonymous]
    [Authorize]
    public string TestAnon()
    {
        var test = User.Identities.ToList().ToString();

        return "Test method has run.";
    }

    [HttpGet("/test - bearer")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public string Test()
    {
        var test = User.Identities.ToList().ToString();

       return "Test method has run.";
    }

    [HttpGet("/test - authorize")]
    [Authorize]
    public string Test2()
    {
        var test = User.Identities.ToList().ToString();

        return "Test method has run.";
    }

    [HttpGet("/test - role")]
    [Authorize(Roles = "User")]
    public string Test3()
    {
        var test = User.Identities.ToList().ToString();

        return "Test method has run.";
    }

    [HttpGet("/test - role2")]
    [Authorize(Roles = "User", AuthenticationSchemes = "Bearer")]
    public string Test3x()
    {
        var test = User.Identities.ToList().ToString();

        return "Test method has run.";
    }

    [HttpGet("/test - role Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public string Test4()
    {
        var test = User.Identities.ToList().ToString();

        return "Test method has run.";
    }
}
