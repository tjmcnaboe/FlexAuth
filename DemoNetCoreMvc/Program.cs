using DemoNetCoreMvc;
using DemoNetCoreMvc.Data;
using FlexAuth.Authorization.Core;
using FlexAuth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<RoleProviderRequestContext>();
builder.Services.AddScoped<IRequestRoleProvider>(provider => provider.GetRequiredService<RoleProviderRequestContext>());


builder.Services.AddSingleton<IFlexPolicy, RootFlexPolicy>();
builder.Services.AddScoped<IGenericPermissionRoleProvider<RootPermissions>, RootRoleProvider>(); // provides the permissions assoscatied with a role
builder.Services.AddScoped<IAuthorizationHandler, InterfacePermissionAuthorizationHandler<RootPermissions>>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, MultiFlexibleAuthorizationPolicyProvider>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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
app.MapRazorPages();

app.Run();
