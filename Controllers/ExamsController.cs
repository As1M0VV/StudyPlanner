using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyPlanner.Data;
using StudyPlanner.Models;

namespace StudyPlanner.Controllers;

public class ExamsController : Controller
{
    private readonly AppDbContext _context;

    public ExamsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? q)
    {
        var exams = _context.Exams
            .Include(exam => exam.Course)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            exams = exams.Where(exam =>
                exam.Title.Contains(q) ||
                (exam.Topics != null && exam.Topics.Contains(q)) ||
                (exam.Course != null && exam.Course.Name.Contains(q)));
        }

        ViewBag.Query = q ?? string.Empty;

        return View(await exams
            .OrderBy(exam => exam.ExamDate)
            .ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        await PopulateCourses();
        return View(new Exam { ExamDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,ExamDate,Topics,Grade,CourseId")] Exam exam)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCourses(exam.CourseId);
            return View(exam);
        }

        _context.Add(exam);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exam = await _context.Exams.FindAsync(id);
        if (exam == null)
        {
            return NotFound();
        }

        await PopulateCourses(exam.CourseId);
        return View(exam);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ExamDate,Topics,Grade,CourseId")] Exam exam)
    {
        if (id != exam.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateCourses(exam.CourseId);
            return View(exam);
        }

        try
        {
            _context.Update(exam);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ExamExists(exam.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var exam = await _context.Exams
            .Include(item => item.Course)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (exam == null)
        {
            return NotFound();
        }

        return View(exam);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam != null)
        {
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCourses(int? selectedCourseId = null)
    {
        var courses = await _context.Courses
            .OrderBy(course => course.Name)
            .ToListAsync();

        ViewBag.CourseId = new SelectList(courses, "Id", "Name", selectedCourseId);
    }

    private async Task<bool> ExamExists(int id)
    {
        return await _context.Exams.AnyAsync(exam => exam.Id == id);
    }
}
