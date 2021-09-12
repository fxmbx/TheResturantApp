
using Resturant.Web;
using Resturant.Web.Services;
using Resturant.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using System;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<IProductService, ProductService>();
SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
SD.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
builder.Services.AddScoped<IProductService,ProductService>();


    builder.Services.AddAuthentication(x=>
    {
        x.DefaultScheme = "Cookies";
        x.DefaultChallengeScheme = "oidc"; 
    }).AddCookie("Cookie", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
        .AddOpenIdConnect("oidc", x => 
        {
            x.Authority = builder.Configuration["ServiceUrls:IdentityAPI"];
            x.GetClaimsFromUserInfoEndpoint = true;
            x.ClientId = "resturant"; //client id in identityapi
            x.ClientSecret = "secret"; //secret from identityapi
            x.ResponseType = "code"; 
            x.RequireHttpsMetadata = false;
            // x.ClaimsActions.MapJsonKey("role","role","role");
            // x.ClaimsActions.MapJsonKey("sub","sub","sub");
            x.TokenValidationParameters.NameClaimType = "name";
            x.TokenValidationParameters.RoleClaimType = "role";
            x.Scope.Add("resturant");
            x.SaveTokens = true;
        });


        
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
