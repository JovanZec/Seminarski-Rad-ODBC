namespace OdbcIS.BLL.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string IndexNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int StudyYear { get; set; }
}
