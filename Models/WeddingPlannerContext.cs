	using Microsoft.EntityFrameworkCore;
	namespace WeddingPlanner.Models
	{
		public class WeddingPlannerContext : DbContext
		{
		// base() calls the parent class' constructor passing the "options" parameter along
		public WeddingPlannerContext(DbContextOptions<WeddingPlannerContext> options) : base(options) { }
		public DbSet<User> users {get;set;}
		public DbSet<Wedding> weddings { get; set; }
		public DbSet<WeddingAttendance> weddingattendance { get; set; }

		}

	
}