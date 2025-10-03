
using DataAccessObjects.Dto;

public class StudentAnalysisDto
{
    public Guid StudentId { get; set; }
    public Guid TermId { get; set; }    // thêm để gắn với học kỳ
    public string FullName { get; set; } = string.Empty;

    // Tổng quan
    public decimal Average { get; set; }
    public int BelowCount { get; set; }
    public string RiskLevel { get; set; } = "low"; // low, medium, high
    public string Comment { get; set; } = string.Empty;

    // Chi tiết từng môn
    public List<SubjectAnalysisDto> Subjects { get; set; } = new();

    // Tóm tắt báo cáo
    public string Summary { get; set; } = string.Empty;
}


