using CurrencyConverter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Context
{
    public class CurrencyDbContext : DbContext
    {
        public DbSet<CurrencyModel> CurrencyModels { get; set; }
    }
}
