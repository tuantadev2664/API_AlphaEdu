//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataAccessObjects
//{
//    public class StudentAnalysisDto
//    {
//        public Guid StudentId { get; set; }
//        public string FullName { get; set; } = string.Empty;
//        public decimal Average { get; set; }
//        public int BelowCount { get; set; }
//        public string RiskLevel { get; set; } = "Low";
//        public string Comment { get; set; } = string.Empty;
//        public Dictionary<string, decimal?> Transcript { get; set; } = new();
//    }

//}


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

public class SubjectAnalysisDto
{
    public decimal? Average { get; set; }
    public int AssignmentsCount { get; set; }
    public int BelowThresholdCount { get; set; }
    public string RiskLevel { get; set; } = "Low";
    public string Comment { get; set; } = string.Empty;
}
