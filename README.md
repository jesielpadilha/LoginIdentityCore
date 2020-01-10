# 1 - Install the follow Packages: 
Microsoft.EntityFrameworkCore  
Microsoft.EntityFrameworkCore.Design  
Microsoft.EntityFrameworkCore.SqlServer  
Microsoft.EntityFrameworkCore.Tools  
Microsoft.AspNetCore.Identity  
Microsoft.AspNetCore.Identity.EntityFrameworkCore

# 2 - Create DbContext and User Class in "Models":

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
	
# 3 - Configure StringConnection in "appsettings.json":

	"ConnectionStrings": {
		"DefaultConnection": "Server=titanium\\sqlexpress;Database=LoginIdentityCore;User Id=sa;Password=htd@2019;Trusted_Connection=False;MultipleActiveResultSets=true"
	}

# 4 - Configure Database and  Identity services in "Starup.cs":
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

		options.LogoutPath = "/Home/Logout"; //Logout page
		options.LoginPath = "/Home/Index"; //Default page after login
		options.AccessDeniedPath = "/Erro/AcessoNegado"; //when access is denied, the user redict to this page
		options.SlidingExpiration = true;
	});

Configure:

	app.UseAuthentication();
	app.UseAuthorization();

# 5 - Execute "add-migration MigrationInitial" and "update-database" to create the Database

# 6 - Create Register and Login services
