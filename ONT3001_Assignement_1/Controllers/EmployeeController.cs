using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONT3001_Assignement_1.Data;
using ONT3001_Assignement_1.Models;

namespace ONT3001_Assignement_1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieve all  employees including their departments
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();
            return View(employees); // Return the Index view associated with employees
        }

        // Search employees by department name
        public async Task<IActionResult> Search(string departmentName)
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e =>  e.Department.Name.Contains(departmentName)) // Filter by department name
                .ToListAsync();

            return View("Index", employees); 
        }

        // Filter employees by age
        public async Task<IActionResult> FilterByAge(int age)
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Age > age) // Fetch employees who are over the specified age 
                .ToListAsync();

            return View("Index", employees); 
        }

        // Get employee details by ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeID == id && e.IsActive); 
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            ViewBag.DepartmentsList = new SelectList(_context.Departments.ToList(), "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee); // IsActive is true by default
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect back to the employees index
            }

            // Repopulate dropdown if model is incorrect
            ViewBag.DepartmentsList = new SelectList(_context.Departments.ToList(), "DepartmentID", "DepartmentName");
            return View(employee); 
        }

        // GET: Employee/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || !employee.IsActive)
                return NotFound();

            ViewBag.DepartmentsList = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // POST: Employee/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.EmployeeID == id))
                        return NotFound();
                    else
                        throw; // Rethrows the exception for further handling
                }
            }

            ViewBag.DepartmentsList = new SelectList(_context.Departments, "DepartmentID", "DepartmentName", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employee/Delete (Soft Delete)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == id && e.IsActive);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // POST: Employee/DeleteConfirmed (Soft Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = false; // Is active is set to false
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }

}

