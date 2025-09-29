
using DataAccessObjects.Dto;

public class StudentAnalysisDto
{
    public Guid StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;

    // Điểm tổng quan
    public decimal Average { get; set; }
    public int BelowCount { get; set; }
    public string RiskLevel { get; set; } = "Low";
    public string Comment { get; set; } = string.Empty;

    // Chi tiết từng môn
    public Dictionary<string, SubjectAnalysisDto> Subjects { get; set; } = new();

    // Tóm tắt về tiến độ học tập
    public string Summary { get; set; } = string.Empty;
}

