using ConsoleApp1.Models;
using ConsoleApp1.Services;
using System.Text.Json;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            string filePath = "E:\\New folder (32)\\Clean Arc\\ConsoleApp1\\ConsoleApp1\\deliveries.json\\deliveries.json";

            // 1. Edge Case: التأكد من وجود الملف
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[Error] File '{filePath}' not found. Please ensure the file is in the output directory.");
                return;
            }

            // 2. قراءة الملف وتحويل البيانات إلى Objects
            string jsonString = File.ReadAllText(filePath);
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Delivery>? deliveries = JsonSerializer.Deserialize<List<Delivery>>(jsonString, jsonOptions);

            // Edge Case: الملف فاضي
            if (deliveries == null || deliveries.Count == 0)
            {
                Console.WriteLine("[Notice] No delivery requests found in the input file.");
                return;
            }

            // 3. تنظيم الرحلات بواسطة الخوارزمية
            var planner = new DeliveryPlanner();
            var trips = planner.OrganizeTrips(deliveries, out var invalidDeliveries);

            // 4. عرض النتائج على الـ Console
            Console.WriteLine("==================================================");
            Console.WriteLine("          DELIVERY ROUTE PLANNER RESULTS          ");
            Console.WriteLine("==================================================\n");

            foreach (var trip in trips)
            {
                Console.WriteLine($"🚚 Trip #{trip.TripId}");
                Console.WriteLine($"   Total Weight: {trip.TotalWeight:F1} kg / 10.0 kg");
                Console.WriteLine($"   Efficiency:   {trip.CapacityEfficiency:F1}%");
                Console.WriteLine("   Deliveries Included:");

                foreach (var d in trip.Deliveries)
                {
                    Console.WriteLine($"     • ID: {d.Id,-2} | Area: {d.Area,-11} | Priority: {d.Priority} | Weight: {d.Weight} kg");
                }
                Console.WriteLine(new string('-', 50));
            }

            // 5. طباعة الشحنات المرفوضة (Edge Cases)
            if (invalidDeliveries.Count > 0)
            {
                Console.WriteLine("\n⚠️ REJECTED DELIVERIES (Exceeds Capacity / Invalid):");
                foreach (var invalid in invalidDeliveries)
                {
                    Console.WriteLine($"   • ID: {invalid.Id} | Area: {invalid.Area} | Weight: {invalid.Weight} kg");
                }
                Console.WriteLine(new string('-', 50));
            }
        }
    }
}
