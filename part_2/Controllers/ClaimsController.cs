using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using part_2.Data;
using part_2.Models;

namespace part_2.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AppDbContext _context;

        public ClaimsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Claims
        public async Task<IActionResult> Index()
        {
            var claims = await _context.Claims.ToListAsync();
            return View(await _context.Claims.ToListAsync());

        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // GET: Claims/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FacultyName,ModuleName,Sessions,TotalAmount,SupportingDocuments")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(claim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(claim);
        }

        // GET: Claims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FacultyName,ModuleName,Sessions,TotalAmount,SupportingDocuments")] Claim claim)
        {
            if (id != claim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(claim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaimExists(claim.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(claim);
        }

        // GET: Claims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims
                .FirstOrDefaultAsync(m => m.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim != null)
            {
                _context.Claims.Remove(claim);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClaimExists(int id)
        {
            return _context.Claims.Any(e => e.Id == id);
        }

     

        public IActionResult CoordinatorDashboard()
        {
            var claims = _context.Claims
                .Where(c => c.Status == "Pending")
                .Select(c => new Claim
                {
                    Id = c.Id,
                    FacultyName = c.FacultyName,
                    ModuleName = c.ModuleName,
                    Sessions = c.Sessions,
                    TotalAmount = c.TotalAmount,
                    SupportingDocuments = c.SupportingDocuments,
                    Status = c.Status
                }).ToList();

            return View(claims); // View: Views/Claims/CoordinatorDashboard.cshtml
        }

        public IActionResult ManagerDashboard()
        {
            var claims = _context.Claims
                .Where(c => c.Status == "Pending")
                .Select(c => new Claim
                {
                    Id = c.Id,
                    FacultyName = c.FacultyName,
                    ModuleName = c.ModuleName,
                    Sessions = c.Sessions,
                    TotalAmount = c.TotalAmount,
                    SupportingDocuments = c.SupportingDocuments,
                    Status = c.Status
                }).ToList();

            return View(claims); // View: Views/Claims/ManagerDashboard.cshtml
        }

        public IActionResult LecturerDashboard() {


            return View();
        }

        
        [HttpPost]
        public IActionResult ApproveManagerClaim(int id)
        {
            var claim = _context.Claims.Find(id);
            if (claim == null)
                return NotFound();

            claim.Status = "Approved";
            _context.SaveChanges();

            return RedirectToAction("ManagerDashboard");
        }

        [HttpPost]
        public IActionResult ApproveCoordinatorClaim(int id)
        {
            var claim = _context.Claims.Find(id);
            if (claim == null)
                return NotFound();

            claim.Status = "Approved";
            _context.SaveChanges();

            return RedirectToAction("CoordinatorDashboard");
        }

        [HttpPost]
        public IActionResult RejectManagerClaim(int id)
        {
            var claim = _context.Claims.Find(id);
            if (claim == null)
                return NotFound();

            claim.Status = "Rejected";
            _context.SaveChanges();

            return RedirectToAction("ManagerDashboard");
        }

        [HttpPost]
        public IActionResult RejectCoordinatorClaim(int id)
        {
            var claim = _context.Claims.Find(id);
            if (claim == null)
                return NotFound();

            claim.Status = "Rejected";
            _context.SaveChanges();

            return RedirectToAction("CoordinatorDashboard");
        }
        public IActionResult TrackClaim(int id)
        {

            // Retrieve the claim with the specified id from your database
            var claim = _context.Claims.FirstOrDefault(c => c.Id == id);
            if (claim == null)
            {
                return NotFound(); // Handle case where claim isn't found
            }

            // Pass the claim to a view to display details
            return View(claim);
        }
    }
}
