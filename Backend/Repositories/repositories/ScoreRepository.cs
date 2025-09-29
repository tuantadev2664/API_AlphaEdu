


using BusinessObjects.Models;
using DataAccessObjects;
using DataAccessObjects.Dto;
using Repositories.interfaces;
using Repositories.repositories;

public class ScoreRepository : Repository<Score>, IScoreRepository
{
    private readonly ScoreDAO _scoreDAO;

    public ScoreRepository(SchoolDbContext context) : base(new ScoreDAO(context))
    {
        _scoreDAO = new ScoreDAO(context);
    }

    public Task<List<ScoreDto>> GetScoresByStudentAsync(Guid studentId) =>
        _scoreDAO.GetScoresByStudentAsync(studentId);

    public Task<List<ScoreDto>> GetScoresByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId) =>
        _scoreDAO.GetScoresByStudentAndSubjectAsync(studentId, subjectId, termId);

    public Task<List<ScoreDto>> GetScoresByClassAndTermAsync(Guid classId, Guid termId) =>
        _scoreDAO.GetScoresByClassAndTermAsync(classId, termId);

    public Task<bool> ScoreExistsAsync(Guid assessmentId, Guid studentId) =>
        _scoreDAO.ScoreExistsAsync(assessmentId, studentId);

    public Task<decimal?> GetAverageScoreByStudentAndSubjectAsync(Guid studentId, Guid subjectId, Guid termId) =>
        _scoreDAO.GetAverageScoreByStudentAndSubjectAsync(studentId, subjectId, termId);

    public Task<Dictionary<string, decimal?>> GetTranscriptByStudentAsync(Guid studentId, Guid termId) =>
        _scoreDAO.GetTranscriptByStudentAsync(studentId, termId);

    public Task<List<StudentScoresDto>> GetStudentScoresByClassAndSubjectAsync(Guid classId, Guid subjectId, Guid termId)=> _scoreDAO.GetStudentScoresByClassAndSubjectAsync(classId, subjectId, termId);   
}