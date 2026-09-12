using System.Data.Common;
using System.Data.Odbc;
using OdbcIS.DAL.Data;
using OdbcIS.DAL.Entities;

namespace OdbcIS.DAL.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly OdbcConnectionFactory _connectionFactory;

    public StudentRepository(OdbcConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<Student>> GetAllAsync()
    {
        const string sql = "SELECT Id, FirstName, LastName, IndexNumber, Email, StudyYear FROM Students ORDER BY LastName, FirstName";
        var students = new List<Student>();

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var command = new OdbcCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            students.Add(MapStudent(reader));

        return students;
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        const string sql = "SELECT Id, FirstName, LastName, IndexNumber, Email, StudyYear FROM Students WHERE Id = ?";

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var command = new OdbcCommand(sql, connection);
        command.Parameters.Add("@Id", OdbcType.Int).Value = id;

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? MapStudent(reader) : null;
    }

    public async Task<bool> IndexNumberExistsAsync(string indexNumber, int? exceptId = null)
    {
        var sql = "SELECT COUNT(*) FROM Students WHERE IndexNumber = ?";
        if (exceptId.HasValue)
            sql += " AND Id <> ?";

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var command = new OdbcCommand(sql, connection);
        command.Parameters.Add("@IndexNumber", OdbcType.NVarChar, 30).Value = indexNumber;
        if (exceptId.HasValue)
            command.Parameters.Add("@Id", OdbcType.Int).Value = exceptId.Value;

        var result = Convert.ToInt32(await command.ExecuteScalarAsync());
        return result > 0;
    }

    public async Task<int> CreateAsync(Student student)
    {
        const string sql = @"
INSERT INTO Students (FirstName, LastName, IndexNumber, Email, StudyYear)
VALUES (?, ?, ?, ?, ?);
SELECT CAST(SCOPE_IDENTITY() AS INT);";

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var command = new OdbcCommand(sql, connection);
        AddStudentParameters(command, student);

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task UpdateAsync(Student student)
    {
        const string sql = @"
UPDATE Students
SET FirstName = ?, LastName = ?, IndexNumber = ?, Email = ?, StudyYear = ?
WHERE Id = ?";

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var command = new OdbcCommand(sql, connection);
        AddStudentParameters(command, student);
        command.Parameters.Add("@Id", OdbcType.Int).Value = student.Id;
        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Students WHERE Id = ?";

        await using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();
        await using var command = new OdbcCommand(sql, connection);
        command.Parameters.Add("@Id", OdbcType.Int).Value = id;
        await command.ExecuteNonQueryAsync();
    }

    private static void AddStudentParameters(OdbcCommand command, Student student)
    {
        // Kod ODBC-a redosled parametara mora da prati redosled ? placeholder-a u SQL-u.
        command.Parameters.Add("@FirstName", OdbcType.NVarChar, 80).Value = student.FirstName;
        command.Parameters.Add("@LastName", OdbcType.NVarChar, 80).Value = student.LastName;
        command.Parameters.Add("@IndexNumber", OdbcType.NVarChar, 30).Value = student.IndexNumber;
        command.Parameters.Add("@Email", OdbcType.NVarChar, 120).Value = student.Email;
        command.Parameters.Add("@StudyYear", OdbcType.Int).Value = student.StudyYear;
    }

    private static Student MapStudent(DbDataReader reader) => new()
    {
        Id = reader.GetInt32(0),
        FirstName = reader.GetString(1),
        LastName = reader.GetString(2),
        IndexNumber = reader.GetString(3),
        Email = reader.GetString(4),
        StudyYear = reader.GetInt32(5)
    };
}
