using System.Linq;
using Judge.Data.Core.Mappings;
using Judge.Model.Core.SubmitSolution;
using Microsoft.EntityFrameworkCore;

namespace Judge.Data
{
    internal sealed class DataContext : DbContext
    {
        public DataContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new LanguageMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new SubmitBaseMapping());
            modelBuilder.ApplyConfiguration(new CheckQueueMapping());
            modelBuilder.ApplyConfiguration(new SubmitResultMapping());
            modelBuilder.ApplyConfiguration(new TaskMapping());
            modelBuilder.ApplyConfiguration(new ContestMapping());
            modelBuilder.ApplyConfiguration(new ContestTaskMapping());
            modelBuilder.ApplyConfiguration(new UserRoleMapping());
        }

        public CheckQueue DequeueSubmitCheck()
        {
            return Set<CheckQueue>().FromSql("EXEC dbo.DequeueSubmitCheck").FirstOrDefault();
        }
    }
}
