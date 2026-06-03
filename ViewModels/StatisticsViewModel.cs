namespace StudyPlanner.ViewModels;

public class StatisticsViewModel
{
    public int TotalStudyMinutes { get; set; }

    public int CompletedTasks { get; set; }

    public int OpenTasks { get; set; }

    public int ExamCount { get; set; }

    public List<CourseStudyStat> CourseStats { get; set; } = new();
}

public class CourseStudyStat
{
    public string CourseName { get; set; } = string.Empty;

    public int TotalMinutes { get; set; }

    public int CompletedTasks { get; set; }

    public int UpcomingExams { get; set; }

    public int Percentage { get; set; }
}
