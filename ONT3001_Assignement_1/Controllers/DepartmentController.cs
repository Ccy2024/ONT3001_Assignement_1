using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONT3001_Assignement_1.Data;

namespace ONT3001_Assignement_1.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Retrieve all active departments
        public async Task<IActionResult> Index()
        {
            var activeDepartments = await _context.Departments
                .Where(d => d.IsActive) // Fetching departments where IsActive is true
                .ToListAsync();
            return View(activeDepartments);
        }

        // Get a specific department by ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var department = await _context.Departments.FindAsync(id);
            if (department == null || !department.IsActive) // Check if department is active
                return NotFound();

            return View(department);
        }

        // GET: Department/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentID == id && d.IsActive); // Only active departments
            if (department == null)
                return NotFound();

            return View(department);
        }

        // POST: Department/DeleteConfirmed (Soft Delete)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                department.IsActive = false; // Soft delete
                _context.Update(department);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

