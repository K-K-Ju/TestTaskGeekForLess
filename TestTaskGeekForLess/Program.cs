using Microsoft.EntityFrameworkCore;
using TestTaskGeekForLess.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TestTaskGeekForLessContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TestTaskGeekForLessContext") ?? throw new InvalidOperationException("Connection string 'TestTaskGeekForLessContext' not found.")));
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    pattern: "{controller=TreeNodes}/{action=Create}");

app.Run();
