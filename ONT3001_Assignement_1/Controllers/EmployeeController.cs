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
                .Where(e => e.IsActive) 
                .Include(e => e.Department)
                .ToListAsync();
            return View(employees);
        }

        public async Task<IActionResult> Search(string departmentName, int? minAge) // Accept both parameters
        {
            var employees = _context.Employees.Where(e => e.IsActive).Include(e => e.Department).AsQueryable(); 

            // Apply department name filter if provided
            if (!string.IsNullOrEmpty(departmentName))
            {
                employees = employees.Where(e => e.Department.Name.Contains(departmentName));
            }

            // Apply age filter if provided
            if (minAge.HasValue)
            {
                employees = employees.Where(e => e.Age >= minAge.Value); 
            }

            var filteredEmployees = await employees.ToListAsync(); 

            return View("Index", filteredEmployees);
        }


        // Get employee details by ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeID == id ); 
            if (employee == null)
                return NotFound();

            return View(employee);
        }


        // GET: Employee/Create
        public IActionResult Create()
        {
            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "DepartmentID", "Name");
            return View();
        }

        // POST: Employee/Create
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
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            // Re-populate dropdown if model is invalid
            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "DepartmentID", "Name", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || !employee.IsActive)
                return NotFound();

            // Make sure this line exists and is executed
            ViewBag.DepartmentList = new SelectList(_context.Departments, "DepartmentID", "Name", employee.DepartmentID);

            return View(employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee updatedEmployee)
        {
            if (id != updatedEmployee.EmployeeID)
                return NotFound();

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        var existingEmployee = await _context.Employees.FindAsync(id);
            //        if (existingEmployee == null || !existingEmployee.IsActive)
            //            return NotFound();

            //        await _context.SaveChangesAsync();

            //        return RedirectToAction(nameof(Index));
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!_context.Employees.Any(e => e.EmployeeID == id && e.IsActive))
            //            return NotFound();
            //        else
            //            throw;
            //    }
            //}
            try
            {
                _context.Update(updatedEmployee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));   
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employees.Any(e => e.EmployeeID == id))
                    return NotFound();
                else
                    throw;
            }

            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "DepartmentID", "Name", updatedEmployee.DepartmentID);
            return View(updatedEmployee);
        }


        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Employee/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int EmployeeID)
        {
            var employee = await _context.Employees.FindAsync(EmployeeID);
            if (employee != null)
            {
                employee.IsActive = false; // Soft delete
                _context.Update(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}



