using OdbcIS.DAL.Entities;

namespace OdbcIS.DAL.Repositories;

public interface IStudentRepository
{
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<bool> IndexNumberExistsAsync(string indexNumber, int? exceptId = null);
    Task<int> CreateAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(int id);
}
