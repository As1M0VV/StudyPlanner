using System.ComponentModel.DataAnnotations;

namespace StudyPlanner.Models;

public class Exam
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Sinav basligi zorunludur.")]
    [StringLength(100, ErrorMessage = "Sinav basligi en fazla 100 karakter olabilir.")]
    [Display(Name = "Sinav Adi")]
    public string Title { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "Sinav Tarihi")]
    public DateTime ExamDate { get; set; } = DateTime.Today;

    [StringLength(500, ErrorMessage = "Konular en fazla 500 karakter olabilir.")]
    [Display(Name = "Konular")]
    public string? Topics { get; set; }

    [Range(0, 100, ErrorMessage = "Not 0 ile 100 arasinda olmalidir.")]
    [Display(Name = "Not")]
    public decimal? Grade { get; set; }

    [Display(Name = "Ders")]
    public int CourseId { get; set; }

    public Course? Course { get; set; }
}
