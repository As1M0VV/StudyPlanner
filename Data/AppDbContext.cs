using Microsoft.EntityFrameworkCore;
using StudyPlanner.Models;

namespace StudyPlanner.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<StudyTask> StudyTasks => Set<StudyTask>();
    public DbSet<Exam> Exams => Set<Exam>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>()
            .HasMany(course => course.StudyTasks)
            .WithOne(task => task.Course)
            .HasForeignKey(task => task.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Course>()
            .HasMany(course => course.Exams)
            .WithOne(exam => exam.Course)
            .HasForeignKey(exam => exam.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Course>()
            .Property(course => course.Name)
            .HasMaxLength(80)
            .IsRequired();

        modelBuilder.Entity<StudyTask>()
            .Property(task => task.Title)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Exam>()
            .Property(exam => exam.Title)
            .HasMaxLength(100)
            .IsRequired();
    }
}
