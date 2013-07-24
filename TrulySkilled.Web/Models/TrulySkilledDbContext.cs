using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TrulySkilled.Web.Models
{
    public class TrulySkilledDbContext : DbContext
    {
        public TrulySkilledDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}