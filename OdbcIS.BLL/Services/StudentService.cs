using System.Net.Mail;
using OdbcIS.BLL.DTOs;
using OdbcIS.DAL.Entities;
using OdbcIS.DAL.Repositories;

namespace OdbcIS.BLL.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<StudentDto>> GetAllAsync()
        => (await _repository.GetAllAsync()).Select(ToDto).ToList();

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        if (id <= 0) return null;
        var entity = await _repository.GetByIdAsync(id);
        return entity is null ? null : ToDto(entity);
    }

    public async Task<int> CreateAsync(StudentDto student)
    {
        Normalize(student);
        Validate(student);

        if (await _repository.IndexNumberExistsAsync(student.IndexNumber))
            throw new InvalidOperationException("Student sa tim brojem indeksa već postoji.");

        return await _repository.CreateAsync(ToEntity(student));
    }

    public async Task UpdateAsync(StudentDto student)
    {
        if (student.Id <= 0)
            throw new ArgumentException("Neispravan ID studenta.");

        Normalize(student);
        Validate(student);

        if (await _repository.GetByIdAsync(student.Id) is null)
            throw new KeyNotFoundException("Student nije pronađen.");

        if (await _repository.IndexNumberExistsAsync(student.IndexNumber, student.Id))
            throw new InvalidOperationException("Student sa tim brojem indeksa već postoji.");

        await _repository.UpdateAsync(ToEntity(student));
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0 || await _repository.GetByIdAsync(id) is null)
            throw new KeyNotFoundException("Student nije pronađen.");

        await _repository.DeleteAsync(id);
    }

    private static void Normalize(StudentDto student)
    {
        student.FirstName = student.FirstName.Trim();
        student.LastName = student.LastName.Trim();
        student.IndexNumber = student.IndexNumber.Trim().ToUpperInvariant();
        student.Email = student.Email.Trim().ToLowerInvariant();
    }

    private static void Validate(StudentDto student)
    {
        if (string.IsNullOrWhiteSpace(student.FirstName) || student.FirstName.Length < 2)
            throw new ArgumentException("Ime mora imati najmanje 2 karaktera.");

        if (string.IsNullOrWhiteSpace(student.LastName) || student.LastName.Length < 2)
            throw new ArgumentException("Prezime mora imati najmanje 2 karaktera.");

        if (string.IsNullOrWhiteSpace(student.IndexNumber))
            throw new ArgumentException("Broj indeksa je obavezan.");

        try { _ = new MailAddress(student.Email); }
        catch { throw new ArgumentException("Email adresa nije ispravna."); }

        if (student.StudyYear is < 1 or > 6)
            throw new ArgumentException("Godina studija mora biti između 1 i 6.");
    }

    private static StudentDto ToDto(Student student) => new()
    {
        Id = student.Id,
        FirstName = student.FirstName,
        LastName = student.LastName,
        IndexNumber = student.IndexNumber,
        Email = student.Email,
        StudyYear = student.StudyYear
    };

    private static Student ToEntity(StudentDto student) => new()
    {
        Id = student.Id,
        FirstName = student.FirstName,
        LastName = student.LastName,
        IndexNumber = student.IndexNumber,
        Email = student.Email,
        StudyYear = student.StudyYear
    };
}
