using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudyPlanner.Data;
using StudyPlanner.Models;

namespace StudyPlanner.Controllers;

public class StudyTasksController : Controller
{
    private readonly AppDbContext _context;

    public StudyTasksController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? status, string? q)
    {
        var tasks = _context.StudyTasks
            .Include(task => task.Course)
            .AsQueryable();

        if (status == "completed")
        {
            tasks = tasks.Where(task => task.IsCompleted);
        }
        else if (status == "open")
        {
            tasks = tasks.Where(task => !task.IsCompleted);
        }

        if (!string.IsNullOrWhiteSpace(q))
        {
            tasks = tasks.Where(task =>
                task.Title.Contains(q) ||
                (task.Course != null && task.Course.Name.Contains(q)));
        }

        ViewBag.Status = status ?? "all";
        ViewBag.Query = q ?? string.Empty;

        return View(await tasks
            .OrderBy(task => task.IsCompleted)
            .ThenBy(task => task.StudyDate)
            .ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        await PopulateCourses();
        return View(new StudyTask { StudyDate = DateTime.Today, DurationMinutes = 60 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,StudyDate,DurationMinutes,IsCompleted,CourseId")] StudyTask studyTask)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCourses(studyTask.CourseId);
            return View(studyTask);
        }

        _context.Add(studyTask);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var studyTask = await _context.StudyTasks.FindAsync(id);
        if (studyTask == null)
        {
            return NotFound();
        }

        await PopulateCourses(studyTask.CourseId);
        return View(studyTask);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StudyDate,DurationMinutes,IsCompleted,CourseId")] StudyTask studyTask)
    {
        if (id != studyTask.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateCourses(studyTask.CourseId);
            return View(studyTask);
        }

        try
        {
            _context.Update(studyTask);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await StudyTaskExists(studyTask.Id))
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

        var studyTask = await _context.StudyTasks
            .Include(task => task.Course)
            .FirstOrDefaultAsync(task => task.Id == id);

        if (studyTask == null)
        {
            return NotFound();
        }

        return View(studyTask);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var studyTask = await _context.StudyTasks.FindAsync(id);
        if (studyTask != null)
        {
            _context.StudyTasks.Remove(studyTask);
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

    private async Task<bool> StudyTaskExists(int id)
    {
        return await _context.StudyTasks.AnyAsync(task => task.Id == id);
    }
}
