using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using dip.Models.Domain;

namespace dip.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        
            
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //---------------------------old part-----------------------------------------------------
        public DbSet<Domain.ActionPro> ActionPros { get; set; }
        public DbSet<Domain.Action> Actions { get; set; }
        public DbSet<ActionSpec> ActionSpecs { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<ActionVrem> ActionVrems { get; set; }
        public DbSet<AllAction> AllActions { get; set; }
        public DbSet<Chain> Chains { get; set; }
        public DbSet<FEAction> FEActions { get; set; }
        public DbSet<FEIndex> FEIndexs { get; set; }
        public DbSet<FEObject> FEObjects { get; set; }
        public DbSet<FizVel> FizVels { get; set; }
        public DbSet<NewFEIndex> NewFEIndexs { get; set; }
        public DbSet<NeZakon> NeZakons { get; set; }
        public DbSet<Pro> Pros { get; set; }
        public DbSet<ReverseChain> ReverseChains { get; set; }
        public DbSet<Spec> Specs { get; set; }
        public DbSet<TasksToSynthesy> TasksToSynthesys { get; set; }
        public DbSet<The> Thes { get; set; }
        public DbSet<ThesChild> ThesChilds { get; set; }
        public DbSet<Vrem> Vrems { get; set; }


        //--------------------------------------------------------------



        //public DbSet<Team> Teams { get; set; }



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}