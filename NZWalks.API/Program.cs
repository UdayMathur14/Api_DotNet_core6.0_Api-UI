using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//to see how our application is works
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NzWalks_Log.text" , rollingInterval :RollingInterval.Minute)
    .MinimumLevel.Warning()
    .CreateLogger();

//clear the providers that we have injected till now 
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{

    //this is the one time setup that we have to make so that we are able to access the authentication and add authorization in the header 
    //when we are accessing the endpoints using swagger , basically use token and auth in swagger rather than postman 
    options.SwaggerDoc("v1",new OpenApiInfo { Title = "nz walks api ",Version = "v1"});
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<NZwalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks")));


//inject auth dbcontext for authenctication 
builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));



builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();
builder.Services.AddScoped<ITokenRepositary, TokenRepositary>();
builder.Services.AddScoped<IImageRepository , LocalImageRepositary>();  



builder.Services.AddAutoMapper(typeof(AutoMappersProfiles));

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();




//These are the options for the password that we want to configure , like how many digits do we want and how many uppercase 
//or lowercase 
//This was the setup required to inject Identity into our solution so that we can so we can use it inside the controller 
//register and login as well 
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;  
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

});


//this way we are creating and adding authentication to our services and also adding the jwtbearer token along with 
//parameters that we want the token to be validated against .

//On what basis we will authenticate out token as well 


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //so once the token has been created what parameters do we validate it against
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Add authentication in the middleware pipeline 
app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath = "/Images"
});

app.MapControllers();

app.Run();



//filtering in asp.net core = 
//suppose you run a query where you get all the data but you want to filter the data for some names so that is called filtering 
//we are giving basically a subset back to the client 

//filter by name or the walk and apply a filter query 

//filtering is generally happen in the get part , so we apply filtering in gelall in the region part 


//sorting in asp.net core web api 
//when client wants to retrive data from the server in the specific order


//pagination 

//to limit the amount of data returned to the client . allows to divide the data into smaller pages which makes it easier for client 
//consume and navigate through the data 



