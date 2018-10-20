


namespace PhysicalEffectsSearchEngine.Models
{
    using dip.Models.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class PhysicalEffectsEntities : DbContext
    {
        public PhysicalEffectsEntities()
            : base("name=PhysicalEffectsEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<FEAction> FEAction { get; set; }
        public virtual DbSet<FEText> FEText { get; set; }
        public virtual DbSet<FizVel> FizVels { get; set; }
        public virtual DbSet<The> Thes { get; set; }
    }
}