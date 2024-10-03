using FrontEnd.Services; // Убедитесь, что вы добавили это пространство имен
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Регистрация HttpClient для AuthService
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https://your-auth-microservice-url/"); // Укажите свой URL
});

// Регистрация контроллеров и представлений
builder.Services.AddControllersWithViews(); // или AddRazorPages() для Razor Pages

var app = builder.Build();

// Настройка middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
