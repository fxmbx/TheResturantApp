using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

using ResturantServices.ShoppingCartAPI;
using ResturantServices.ShoppingCartAPI.Data;
using ResturantServices.ShoppingCartAPI.Repository.IRepository;
using ResturantServices.ShoppingCartAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(x=>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

//adding automapper config
IMapper mapper = AutoMapperProfile.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICartRepo,CartRepo>();
builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", x=>
        {
            x.Authority = "https://localhost:44305";
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
    c.SwaggerDoc("v1", new() { Title = "ResturantServices.ShoppingCartAPI", Version = "v1" });
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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ResturantServices.ShoppingCartAPI v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
