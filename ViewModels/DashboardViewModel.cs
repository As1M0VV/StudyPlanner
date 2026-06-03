using StudyPlanner.Models;

namespace StudyPlanner.ViewModels;

public class DashboardViewModel
{
    public int TotalCourses { get; set; }

    public int TotalTasks { get; set; }

    public int CompletedTasks { get; set; }

    public int TotalStudyMinutes { get; set; }

    public List<StudyTask> UpcomingTasks { get; set; } = new();

    public List<Exam> UpcomingExams { get; set; } = new();
}
