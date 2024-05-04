using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter.Models
{
    public class CurrencyModel
    {
        public int Id { get; set; }
        public float Amount { get; set; }
        public string CurrencyName { get; set; }
    }
}
