using System.Data.Entity;
using GameOfLife.Models;

namespace GameOfLife
{
    class LifeInfo : DbContext
    {
        public static string connStr;
        public LifeInfo()
        {
            this.Database.Connection.ConnectionString = connStr;
        }
        public DbSet<GOLData> GOLDatas { get; set; }
    }
}
