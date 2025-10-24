using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using part_2.Data;
using part_2.Models;

namespace part_2.Controllers
{
    public class RegisterViewsController : Controller
    {
        private readonly AppDbContext _context;

        public RegisterViewsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RegisterViews
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegisterViews.ToListAsync());
        }

        // GET: RegisterViews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registerView = await _context.RegisterViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registerView == null)
            {
                return NotFound();
            }

            return View(registerView);
        }

        // GET: RegisterViews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegisterViews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Password,Role")] RegisterViews registerView)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (_context.RegisterViews.Any(e => e.Email == registerView.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(registerView);
                }

                // Hash the password
                //var passwordHasher = new PasswordHasher<RegisterViews>();
                //registerView.PasswordHash = passwordHasher.HashPassword(registerView, registerView.Password);
                //registerView.Password = null; // Clear plain password


                _context.Add(registerView);
                await _context.SaveChangesAsync();
                // Redirect based on role
                switch (registerView.Role)
                {
                    case "Lecturer":
                        return RedirectToAction("Create", "Claims");
                    case "Program Coordinator":
                        return RedirectToAction("CoordinatorDashboard","Claims");
                    case "Academic Manager":
                        return RedirectToAction("ManagerDashboard", "Claims");
                    default:
                        return RedirectToAction("Index");
                }
           
            }
            return RedirectToAction("Index", "Home");


        }

        // GET: RegisterViews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registerView = await _context.RegisterViews.FindAsync(id);
            if (registerView == null)
            {
                return NotFound();
            }
            return View(registerView);
        }

        // POST: RegisterViews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Password,Role")] RegisterViews registerView)
        {
            if (id != registerView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registerView);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegisterViewExists(registerView.Id))
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
            return View(registerView);
        }

        // GET: RegisterViews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registerView = await _context.RegisterViews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registerView == null)
            {
                return NotFound();
            }

            return View(registerView);
        }

        // POST: RegisterViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registerView = await _context.RegisterViews.FindAsync(id);
            if (registerView != null)
            {
                _context.RegisterViews.Remove(registerView);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegisterViewExists(int id)
        {
            return _context.RegisterViews.Any(e => e.Id == id);
        }


    }
}
