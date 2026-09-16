using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Trip
    {
        public int TripId { get; set; }
        public List<Delivery> Deliveries { get; set; } = new();
        public double TotalWeight => Deliveries.Sum(d => d.Weight);
        public double CapacityEfficiency => (TotalWeight / 10.0) * 100; 
    }
}
