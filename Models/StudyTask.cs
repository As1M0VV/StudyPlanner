using System.ComponentModel.DataAnnotations;

namespace StudyPlanner.Models;

public class StudyTask
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Baslik zorunludur.")]
    [StringLength(100, ErrorMessage = "Baslik en fazla 100 karakter olabilir.")]
    [Display(Name = "Baslik")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Aciklama en fazla 500 karakter olabilir.")]
    [Display(Name = "Aciklama")]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Calisma Tarihi")]
    public DateTime StudyDate { get; set; } = DateTime.Today;

    [Range(15, 600, ErrorMessage = "Sure 15 ile 600 dakika arasinda olmalidir.")]
    [Display(Name = "Sure (Dakika)")]
    public int DurationMinutes { get; set; } = 60;

    [Display(Name = "Tamamlandi")]
    public bool IsCompleted { get; set; }

    [Display(Name = "Ders")]
    public int CourseId { get; set; }

    public Course? Course { get; set; }
}
