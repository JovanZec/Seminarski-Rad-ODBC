using Microsoft.AspNetCore.Mvc;
using OdbcIS.BLL.DTOs;
using OdbcIS.BLL.Services;
using OdbcIS.Web.Models;

namespace OdbcIS.Web.Controllers;

public class StudentsController : Controller
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task<IActionResult> Index()
    {
        var students = (await _studentService.GetAllAsync()).Select(ToViewModel).ToList();
        return View(students);
    }

    public async Task<IActionResult> Details(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student is null ? NotFound() : View(ToViewModel(student));
    }

    [HttpGet]
    public IActionResult Create() => View(new StudentViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StudentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _studentService.CreateAsync(ToDto(model));
            TempData["Success"] = "Student je uspešno dodat preko ODBC konekcije.";
            return RedirectToAction(nameof(Index));
        }
        catch (ArgumentException ex) { ModelState.AddModelError(string.Empty, ex.Message); }
        catch (InvalidOperationException ex) { ModelState.AddModelError(string.Empty, ex.Message); }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student is null ? NotFound() : View(ToViewModel(student));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(StudentViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            await _studentService.UpdateAsync(ToDto(model));
            TempData["Success"] = "Podaci su uspešno izmenjeni.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or KeyNotFoundException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        return student is null ? NotFound() : View(ToViewModel(student));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _studentService.DeleteAsync(id);
            TempData["Success"] = "Student je obrisan.";
        }
        catch (KeyNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private static StudentViewModel ToViewModel(StudentDto dto) => new()
    {
        Id = dto.Id,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        IndexNumber = dto.IndexNumber,
        Email = dto.Email,
        StudyYear = dto.StudyYear
    };

    private static StudentDto ToDto(StudentViewModel model) => new()
    {
        Id = model.Id,
        FirstName = model.FirstName,
        LastName = model.LastName,
        IndexNumber = model.IndexNumber,
        Email = model.Email,
        StudyYear = model.StudyYear
    };
}
