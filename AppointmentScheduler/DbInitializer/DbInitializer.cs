using AppointmentScheduler.Models;
using AppointmentScheduler.Utilty;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppointmentScheduler.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager { get; }
        private RoleManager<IdentityRole> _roleManager { get; }
        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

            }
            catch(Exception)
            {

            }
            if (_db.Roles.Any(x => x.Name == Utilty.Helper.Admin)) return;
           
                 _roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
                 _roleManager.CreateAsync(new IdentityRole(Helper.Doctor)).GetAwaiter().GetResult();
                 _roleManager.CreateAsync(new IdentityRole(Helper.Patient)).GetAwaiter().GetResult();
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Name = "Admin Shakeel"
            },"Admin123@").GetAwaiter().GetResult();
            //if(_userManager.RESU)
            ApplicationUser user = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user, Helper.Admin).GetAwaiter().GetResult();
        }
    }
}
