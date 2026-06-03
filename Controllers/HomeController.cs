using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyPlanner.Data;
using StudyPlanner.Models;
using StudyPlanner.ViewModels;

namespace StudyPlanner.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;

        var model = new DashboardViewModel
        {
            TotalCourses = await _context.Courses.CountAsync(),
            TotalTasks = await _context.StudyTasks.CountAsync(),
            CompletedTasks = await _context.StudyTasks.CountAsync(task => task.IsCompleted),
            TotalStudyMinutes = await _context.StudyTasks
                .Where(task => task.IsCompleted)
                .SumAsync(task => (int?)task.DurationMinutes) ?? 0,
            UpcomingTasks = await _context.StudyTasks
                .Include(task => task.Course)
                .Where(task => !task.IsCompleted && task.StudyDate >= today)
                .OrderBy(task => task.StudyDate)
                .Take(5)
                .ToListAsync(),
            UpcomingExams = await _context.Exams
                .Include(exam => exam.Course)
                .Where(exam => exam.ExamDate >= today)
                .OrderBy(exam => exam.ExamDate)
                .Take(5)
                .ToListAsync()
        };

        return View(model);
    }

    public async Task<IActionResult> Statistics()
    {
        var today = DateTime.Today;
        var courses = await _context.Courses
            .Include(course => course.StudyTasks)
            .Include(course => course.Exams)
            .OrderBy(course => course.Name)
            .ToListAsync();

        var totalMinutes = courses
            .SelectMany(course => course.StudyTasks)
            .Where(task => task.IsCompleted)
            .Sum(task => task.DurationMinutes);

        var model = new StatisticsViewModel
        {
            TotalStudyMinutes = totalMinutes,
            CompletedTasks = courses.SelectMany(course => course.StudyTasks).Count(task => task.IsCompleted),
            OpenTasks = courses.SelectMany(course => course.StudyTasks).Count(task => !task.IsCompleted),
            ExamCount = courses.SelectMany(course => course.Exams).Count(),
            CourseStats = courses.Select(course =>
            {
                var courseMinutes = course.StudyTasks
                    .Where(task => task.IsCompleted)
                    .Sum(task => task.DurationMinutes);

                return new CourseStudyStat
                {
                    CourseName = course.Name,
                    TotalMinutes = courseMinutes,
                    CompletedTasks = course.StudyTasks.Count(task => task.IsCompleted),
                    UpcomingExams = course.Exams.Count(exam => exam.ExamDate >= today),
                    Percentage = totalMinutes == 0
                        ? 0
                        : (int)Math.Round(courseMinutes * 100m / totalMinutes)
                };
            }).ToList()
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
