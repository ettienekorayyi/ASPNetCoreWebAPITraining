using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Databases
{
    public class SqlServerDbContext : BaseDbContext
    {
        private readonly IConfiguration _configuration;
        
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options, 
            IConfiguration configuration)
                : base(options, configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration
                .GetConnectionString(StaticConfiguration.SqlServer));
        }
    }
}