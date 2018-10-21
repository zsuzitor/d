



//namespace PhysicalEffectsSearchEngine.Models
//{
//    using dip.Models.TechnicalFunctions;
//    using System;
//    using System.Data.Entity;
//    using System.Data.Entity.Infrastructure;

//    public partial class TechnicalFunctionsEntities : DbContext
//    {
//        public TechnicalFunctionsEntities()
//            : base("name=TechnicalFunctionsEntities")
//        {
//        }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            throw new UnintentionalCodeFirstException();
//        }

//        public virtual DbSet<Index> Index { get; set; }
//        public virtual DbSet<Limit> Limit { get; set; }
//        public virtual DbSet<Operand> Operand { get; set; }
//        public virtual DbSet<OperandGroup> OperandGroup { get; set; }
//        public virtual DbSet<Operation> Operation { get; set; }
//    }
//}