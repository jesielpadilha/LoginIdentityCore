This tutorial was made using Visual Studio Community 2017 and the commands was realized on NuGet Package Manager Console tool, therefor, if your prefer use Dotnet CLI or use a distinct IDE (e.g. VS Code) is necessary to use the Dotnet CLI specific commands.  
More informations: https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=netcore-cli

## 1 - Install the follow Packages: 
Microsoft.EntityFrameworkCore  
Microsoft.EntityFrameworkCore.Design  
Microsoft.EntityFrameworkCore.SqlServer  
Microsoft.EntityFrameworkCore.Tools (optional when used Dotnet CLI)  
Microsoft.AspNetCore.Identity  
Microsoft.AspNetCore.Identity.EntityFrameworkCore  

## 2 - Create DbContext and User Class in "Models":

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }

    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
    }
	
## 3 - Configure StringConnection in "appsettings.json":

	"ConnectionStrings": {
		"DefaultConnection": "Server=serverAddress;Database=LoginIdentityCore;User Id=username;Password=password;Trusted_Connection=False;MultipleActiveResultSets=true"
	}

## 4 - Configure Database and  Identity services in "Starup.cs":
ConfigureServices:
		
    services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

    services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => {
        //Passwork configuration
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 4;
        options.Password.RequiredUniqueChars = 0;

        //Lock configuration
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        //User configuration
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        options.User.RequireUniqueEmail = false;
        }).AddEntityFrameworkStores<ApplicationDbContext>();

    services.AddRouting(o => o.LowercaseUrls = true);		

    services.ConfigureApplicationCookie(options =>
    {
        //Cookies configuration
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

        options.LogoutPath = "/User/Logout"; //Logout page
        options.LoginPath = "/User/Index"; //Default page after login
        options.AccessDeniedPath = "/User/AccessDenied"; //when access is denied, the user redict to this page
        options.SlidingExpiration = true;
    });

Configure:

    app.UseAuthentication();
    app.UseAuthorization();

## 5 - Execute "add-migration MigrationInitial" and "update-database" to create the Database

## 6 - Create Register and Login services (pages, controller and necessary classes)

---
# Working with User Roles:
## 1 - Add Seed method in order to insert the necessary Roles in Database (this process can be executed manually, directly on Database via Script SQL).

Class with static parameters that represent the users categories. These data will be insert on Database and use in different parts of the system.

    public class UserCategory
    {
        public static Guid AdminId => new Guid("1865a4ce-5c68-4128-bbc4-0d4b1225a704");
        public const string AdminDescription = "Administrator";
    }

**ModelBuilderExtensions.cs** (It's not necessary create an specific class to handle this operation, that was made to keep Code clean)</p>

    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            //insert All necessary Roles
            builder.Entity<IdentityRole<Guid>>().HasData(new
            {
                Id = UserCategory.AdminId,
                Name = UserCategory.AdminDescription,
                NormalizedName = UserCategory.AdminDescription.ToUpper()
            });
        }
    }

**ApplicationDbContext.cs**

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Seed();
    }
## 2 - Create a new Migration and Update de Database, to apply the changes

## 3 - add the UserRoles when registering an User
    await _userManager.AddToRoleAsync(user, UserCategory.AdminDescription);

## 4 - In Controller or in a specific Action Add the necessary UserRoles to grant access for these Services.

    [Authorize(Roles = UserCategory.AdminDescription)]
    public IActionResult Privacy()
    {
        return View();
