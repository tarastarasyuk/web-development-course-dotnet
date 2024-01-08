using System.Reflection;
using TheaterCashRegister.API.PL.Mappings;
using TheaterCashRegister.BLL.MappingProfiles;
using TheaterCashRegister.BLL.Service;
using TheaterCashRegister.BLL.Service.IService;
using TheaterCashRegister.DAL.Data;
using TheaterCashRegister.DAL.Repository;
using TheaterCashRegister.DAL.Repository.IRepository;
using TheaterCashRegister.SSR.PL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<DefaultExceptionFilterAttribute>();
});
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

//Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        configurePolicy =>
        {
            configurePolicy
                .AllowAnyOrigin() 
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAllOrigins");

app.Run();