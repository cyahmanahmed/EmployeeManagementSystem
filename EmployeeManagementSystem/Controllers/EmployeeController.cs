using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index(int? minAge)
        {
            var query = _context.Employee
                            .Include(e => e.Department)
                            .Where(e => e.IsActive);

            // Filter by age if provided
            if (minAge.HasValue)
            {
                query = query.Where(e => e.Age > minAge.Value);
            }

            var employees = await query.ToListAsync();

            return View(employees);
        }

        // GET: Employee Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        //Search
        public async Task<IActionResult> Search(string query)
        {
            var filteredEmployee = await _context.Employee
                                  .Include(e => e.Department)
                                  .Where(e => e.Department.DepartmentName.Contains(query))
                                  .ToListAsync();

            return View("Index", filteredEmployee);
        }





        // GET: Employee Create
        public IActionResult Create()
        {
            ViewBag.DepartmentList = new SelectList(_context.Department.ToList(), "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: Employee Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            catch (DbUpdateException)
            {
                //Log the error (uncomment ex variable name and write a log.)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");

            }

            // Repopulate dropdown if model is invalid
            ViewBag.DepartmentList = new SelectList(_context.Department.ToList(), "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employee Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null) return NotFound();

            ViewBag.DepartmentList = new SelectList(_context.Department, "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // POST: Employee Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeID)
                return NotFound();


            try
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employee.Any(p => p.EmployeeID == id))
                    return NotFound();
                else
                    throw;
            }


            ViewBag.DepartmentList = new SelectList(_context.Department, "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employee Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Employee
                .Include(p => p.Department)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (employee == null) return NotFound();

            return View(employee);
        }

        // POST: Employee Soft Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = false;
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.EmployeeID == id);
        }
    }
}