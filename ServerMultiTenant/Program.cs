using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerMultiTenant;
using ServerMultiTenant.Areas.Identity;
using ServerMultiTenant.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

// Add MultiTenant
builder.Services.AddMultiTenant<TenantInfo>()
                    .WithConfigurationStore()
                    .WithHostStrategy();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ContextHelper>();
var app = builder.Build();

// Apply migrations if needed
var store = app.Services.GetRequiredService<IMultiTenantStore<TenantInfo>>();
foreach (var tenant in await store.GetAllAsync())
{
    await using var db = new ApplicationDbContext(tenant);
    await db.Database.MigrateAsync();

    db.ToDoItems.Add(new ToDoItem { Title = "Send Invoices Locally", Completed = true });
    db.ToDoItems.Add(new ToDoItem { Title = "Do not Pay Salaries", Completed = true });
    db.ToDoItems.Add(new ToDoItem { Title = "Do not Write any Memos", Completed = true });
    db.ToDoItems.Add(new ToDoItem { Title = "Forget any Memos", Completed = true });
    await db.SaveChangesAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseMultiTenant();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
