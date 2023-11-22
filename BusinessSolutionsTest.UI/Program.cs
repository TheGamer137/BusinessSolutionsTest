using BusinessSolutionsTest.Core.Repositories;
using BusinessSolutionsTest.Infrastructure;
using BusinessSolutionsTest.Infrastructure.Repositories;
using BusinessSolutionsTest.UI.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSqlServer<AppDbContext>(builder.Configuration.GetConnectionString("SqlServerConnection"))
    .AddScoped<IOrderRepository, OrderRepository>()
    .AddAutoMapper(c => c.AddProfile<OrderMappingProfile>())
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Order}/{action=Index}/{id?}");

app.Run();