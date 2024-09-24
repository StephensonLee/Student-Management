using Student_Management.Models;
using Microsoft.EntityFrameworkCore;
using Student_Management.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Student_Management;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Student_Management.Filters;
using Student_Management.Sevices;
using Student_Management.Midware;
using Microsoft.Build.Framework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>{options.Filters.Add<ModelValidationFilter>();});

builder.Services.AddDbContext<StudentManagementContext>(options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:key"]))
        };
    });

builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));
//builder.Services.AddScoped<IEntityService<Category>, EntityService<Category>>;
//builder.Services.AddTransient<ExceptionHandling>();
//builder.Services.AddAutoMapper(typeof());
var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseRouting();

    //app.UseEndpoints(endpoints =>
    //{
    //    endpoints.MapControllers();
    //});
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandling>();
app.MapControllers();
app.MapRazorPages();

app.Run();
