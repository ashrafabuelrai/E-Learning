using E_Learning.API.Middleware;
using E_Learning.Application.Interfaces;
using E_Learning.Application.Services.Implementation;
using E_Learning.Application.Services.Interface;
using E_Learning.Infrastructure.Data;
using E_Learning.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ILearnerService, LearnerService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "E-learning API",
        Version = "v1",
        Description = "E-learning platform API for government entities. Manage courses, learners, and enrollment workflows.",
        Contact = new OpenApiContact { Name = "API Support" }
    });

    c.AddSecurityDefinition("RoleHeader", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-User-Role",
        Type = SecuritySchemeType.ApiKey,
        Description = "Simulated user role: Admin | Manager | Learner"
    });

    c.AddSecurityDefinition("UserIdHeader", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-User-Id",
        Type = SecuritySchemeType.ApiKey,
        Description = "Simulated user ID (integer)"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference =  new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "RoleHeader" }
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "UserIdHeader" }
            },
            Array.Empty<string>()
        }
    });

    
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseGlobalExceptionMiddleware();
app.UseUserContextMiddleware();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-learning API v1");
        c.RoutePrefix = "swagger"; 
    });
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();