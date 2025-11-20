using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using part_2.Data;
using part_2.Models;

namespace part_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;



        public HomeController(ILogger<HomeController> logger,AppDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
       
        [HttpPost]
        public async Task<IActionResult> LoginDashboard(string Email, string Password)
        {
            // Find user by email
            var user = await _context.RegisterViews.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null)
            {
                // Invalid email
                ModelState.AddModelError("", "Invalid email or password");
                return View("Index"); // Show login again with error
            }

            // Successful login, redirect based on role
            switch (user.Role)
            {
                case "Lecturer":
                    _logger.LogInformation("Redirecting to LecturerDashboard");
                    return RedirectToAction("Create", "Claims");
                case "Program Coordinator":
                    _logger.LogInformation("Redirecting to CoordinatorDashboard");
                    return RedirectToAction("CoordinatorDashboard", "Claims");
                case "Academic Manager":
                    _logger.LogInformation("Redirecting to ManagerDashboard");
                    return RedirectToAction("ManagerDashboard", "Claims");
                case "Human Resources":
                    _logger.LogInformation("Redirecting to HumanResources");
                    return RedirectToAction("HumanResources", "Claims");
                default:
                    _logger.LogInformation("Redirecting to Index");
                    return RedirectToAction("Index"); 
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadSupportingDocument(IFormFile file)
        {


            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "No file selected." });

            // Validate size
            if (file.Length > 5 * 1024 * 1024)
                return Json(new { success = false, message = "File exceeds size limit." });

            // Validate type
            var permittedTypes = new[] {
        "application/pdf",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
    };
            if (!permittedTypes.Contains(file.ContentType))
                return Json(new { success = false, message = "Invalid file type." });

            // Save file securely
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);
            var originalFileName = Path.GetFileName(file.FileName);

            // Optional: To prevent overwriting, you could add a unique suffix if a file with the same name exists
            var filePath = Path.Combine(uploadsFolder, originalFileName);

            if (System.IO.File.Exists(filePath))
            {
                // Append a timestamp or GUID to make filename unique
                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);
                var extension = Path.GetExtension(originalFileName);
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var uniqueFileName = $"{fileNameWithoutExt}_{timestamp}{extension}";
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
            }
            else
            {
                // Save with original filename
                var uniqueFileName = originalFileName;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the filename to store in the claim
            return Json(new { success = true, fileName = Path.GetFileName(filePath) });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
