using OdbcIS.BLL.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("OdbcDatabase")
    ?? throw new InvalidOperationException("ConnectionStrings:OdbcDatabase nije podešen.");

// Prezentacioni sloj registruje samo BLL; detalji DAL-a ostaju skriveni iza BLL ekstenzije.
builder.Services.AddOdbcApplication(connectionString);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();
