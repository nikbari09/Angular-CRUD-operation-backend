using FirstApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace FirstApp.DataContect
{
    public class MyDataContext:DbContext
    {
        public MyDataContext(DbContextOptions options):base(options) { }
                
        public DbSet<Employees> Employees { get; set; }
    }
}
