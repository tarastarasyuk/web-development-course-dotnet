using System.Reflection;
using TheaterCashRegister.BLL.MappingProfiles;
using TheaterCashRegister.BLL.Service;
using TheaterCashRegister.BLL.Service.IService;
using TheaterCashRegister.DAL.Data;
using TheaterCashRegister.DAL.Repository;
using TheaterCashRegister.DAL.Repository.IRepository;
using TheaterCashRegister.SSR.PL;
using TheaterCashRegister.SSR.PL.Filters;
using TheaterCashRegister.SSR.PL.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ApplicationDbContextFactory, ApplicationDbContextFactory>();
builder.Services.AddScoped<ApplicationDbContext>(provider =>
    provider.GetService<ApplicationDbContextFactory>()?.CreateDbContext(args)!);
    
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ITicketService, TicketService>();
builder.Services.AddTransient<IPerformanceService, PerformanceService>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<TicketProfile>();
    cfg.AddProfile<PerformanceProfile>();
    cfg.AddProfile<BookingProfile>();
    cfg.AddProfile<MappingProfile>();
}, Assembly.GetExecutingAssembly());

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();