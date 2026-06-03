using System.ComponentModel.DataAnnotations;

namespace StudyPlanner.Models;

public class Course
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ders adi zorunludur.")]
    [StringLength(80, ErrorMessage = "Ders adi en fazla 80 karakter olabilir.")]
    [Display(Name = "Ders Adi")]
    public string Name { get; set; } = string.Empty;

    [StringLength(80, ErrorMessage = "Ogretim uyesi en fazla 80 karakter olabilir.")]
    [Display(Name = "Ogretim Uyesi")]
    public string? Instructor { get; set; }

    [Range(1, 10, ErrorMessage = "Kredi 1 ile 10 arasinda olmalidir.")]
    [Display(Name = "Kredi")]
    public int Credit { get; set; } = 3;

    public ICollection<StudyTask> StudyTasks { get; set; } = new List<StudyTask>();

    public ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
