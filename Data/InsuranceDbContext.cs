using System;
using InsuranceManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementApp.Data
{
	public class InsuranceDbContext : DbContext
	{
		public DbSet<Insurance> Insurances { get; set; }
		public DbSet<InsuranceEvent> InsuranceEvents { get; set; }
		public DbSet<PolicyHolder> PolicyHolders { get; set; }


        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options)
		: base(options)
		{
		}
    }	
}

