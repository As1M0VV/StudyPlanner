using StudyPlanner.Models;

namespace StudyPlanner.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext dbContext)
    {
        if (dbContext.Courses.Any())
        {
            return;
        }

        var webProgramming = new Course
        {
            Name = "Web Programming",
            Instructor = "Dr. Ayse Demir",
            Credit = 4
        };

        var database = new Course
        {
            Name = "Database Systems",
            Instructor = "Dr. Murat Kaya",
            Credit = 3
        };

        var english = new Course
        {
            Name = "Academic English",
            Instructor = "Lect. Elif Yilmaz",
            Credit = 2
        };

        dbContext.Courses.AddRange(webProgramming, database, english);

        dbContext.StudyTasks.AddRange(
            new StudyTask
            {
                Title = "MVC controller tekrar et",
                Description = "Controller, action ve view baglantisini gozden gecir.",
                StudyDate = DateTime.Today.AddDays(1),
                DurationMinutes = 75,
                IsCompleted = false,
                Course = webProgramming
            },
            new StudyTask
            {
                Title = "Entity Framework iliskileri",
                Description = "One-to-many iliskiler icin kisa not cikar.",
                StudyDate = DateTime.Today.AddDays(-1),
                DurationMinutes = 60,
                IsCompleted = true,
                Course = database
            },
            new StudyTask
            {
                Title = "Presentation hazirligi",
                Description = "Sinif sunumu icin giris metnini duzenle.",
                StudyDate = DateTime.Today.AddDays(3),
                DurationMinutes = 45,
                IsCompleted = false,
                Course = english
            });

        dbContext.Exams.AddRange(
            new Exam
            {
                Title = "Midterm",
                ExamDate = DateTime.Today.AddDays(10),
                Topics = "HTML, CSS, JavaScript, Bootstrap, MVC temelleri",
                Course = webProgramming
            },
            new Exam
            {
                Title = "Quiz 2",
                ExamDate = DateTime.Today.AddDays(5),
                Topics = "SQL sorgulari ve Entity Framework Core",
                Course = database
            },
            new Exam
            {
                Title = "Speaking Exam",
                ExamDate = DateTime.Today.AddDays(14),
                Topics = "Kisa akademik sunum",
                Course = english
            });

        dbContext.SaveChanges();
    }
}
