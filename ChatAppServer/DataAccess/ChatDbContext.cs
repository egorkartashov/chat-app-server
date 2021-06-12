using ChatAppServer.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.DataAccess
{
	public class ChatDbContext : DbContext
	{
		public ChatDbContext(DbContextOptions options) : base(options)
		{
		}
		
		public DbSet<DummyEntity> DummyEntities { get; set; }
	}
}