using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

using ResturantServices.ProductApi;
using ResturantServices.ProductApi.Data;
using ResturantServices.ProductApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ProductDbContext>(x=>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

//adding automapper config
IMapper mapper = AutoMapperProfile.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", x=>
        {
            x.Authority = "https://localhost:44305"; //ssl port of identiy server api
            x.RequireHttpsMetadata = false;
            
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };
        });
builder.Services.AddAuthorization(x => 
        {
            x.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "resturant");
                
            });
        });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "ResturantServices.ProductApi", Version = "v1" });
    //additional swagger configuration 
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
        Description =@"Enter 'Bearer' [space] and your token",
        Name = "Autorization",
        In= ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement ()
    {
        {
        new OpenApiSecurityScheme  
        {
            Reference = new OpenApiReference 
            {
                Type = ReferenceType.SecurityScheme,
                Id= "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In= ParameterLocation.Header
        },
        new List<string>()
        }
    });


});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ResturantServices.ProductApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
