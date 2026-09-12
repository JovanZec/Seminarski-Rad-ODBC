using OdbcIS.BLL.DTOs;

namespace OdbcIS.BLL.Services;

public interface IStudentService
{
    Task<IReadOnlyList<StudentDto>> GetAllAsync();
    Task<StudentDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(StudentDto student);
    Task UpdateAsync(StudentDto student);
    Task DeleteAsync(int id);
}
