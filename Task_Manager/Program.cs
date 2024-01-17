using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using Task_Manager.Core.Abstract.Services;
using Task_Manager.Core.Abstract.Token;
using Task_Manager.Infrastructure;
using Task_Manager.Infrastructure.Concrete;
using Task_Manager.Infrastructure.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//added CORS Policy
builder.Services.AddCors(opt => opt.AddDefaultPolicy(
    policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
    ));


//Db-Context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});


//Identity-Configuration
builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddScoped<ITokenHandler, Task_Manager.Services.Token.TokenHandler>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

});



//jwtTok-authonication
builder.Services.AddHttpContextAccessor();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, //olu�turulacak token de�erini kimlerin/sitelerin kullanaca��n� belirler
            ValidateIssuer = true, //olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r
            ValidateLifetime = true, //Token s�resini kontrol eder do�rulama yeri
            ValidateIssuerSigningKey = true, //�retilecek token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden seciry key verisisinin do�rulamas�d�r.



            ValidAudience = "www.bilmemne.com",
            ValidIssuer = "www.myapi.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ErenCanTuranSecretKeyTaskManager")),
            //jwt �mr�n� belirliyor 15 saniyelik token'� 15saniye sonra etkisiz yap�yor ve eri�imi engelliyor-jwtRefreshToken yap�land�rmas� i�in
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,



            //Log mekan�zmas� i�in
            NameClaimType = ClaimTypes.Name  //JWT �zerinde Name claime kar��l�k gelen de�eri
                                             //User.Identity.Name propertysinden elde edebiliriz.



        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return System.Threading.Tasks.Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"Challenge: {context.Error}, {context.ErrorDescription}");
                return System.Threading.Tasks.Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                Console.WriteLine($"Forbidden: {context.Response.StatusCode}");
                return System.Threading.Tasks.Task.CompletedTask;
            }
        };
    });
//jwtTok-authentication



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();


app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
