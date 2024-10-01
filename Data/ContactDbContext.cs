using System;
using InsuranceManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceManagementApp.Data
{
	public class ContactDbContext : DbContext
	{
		public DbSet<ContactForm> ContactForms { get; set; }

        public ContactDbContext(DbContextOptions<ContactDbContext> options)
        : base(options)
        {
        }

    }
}

