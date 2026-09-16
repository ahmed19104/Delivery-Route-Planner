using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Services
{
    public class DeliveryPlanner
    {
        private const double MaxCapacity = 10.0;

        public List<Trip> OrganizeTrips(List<Delivery> deliveries, out List<Delivery> invalidDeliveries)
        {
            invalidDeliveries = new List<Delivery>();
            var validDeliveries = new List<Delivery>();

            // Edge Case 1 & 2: استبعاد الأحمال الزائدة عن 10 كجم والبيانات غير الصالحة
            foreach (var delivery in deliveries ?? new())
            {
                if (delivery.Weight > MaxCapacity || delivery.Weight <= 0)
                    invalidDeliveries.Add(delivery);
                else
                    validDeliveries.Add(delivery);
            }

            // الترتيب: الأولوية أولاً (الأقل رقماً) ثم المنطقة لتجميع الشحنات المتشابهة
            var sortedDeliveries = validDeliveries
                .OrderBy(d => d.Priority)
                .ThenBy(d => d.Area)
                .ToList();

            var trips = new List<Trip>();
            int tripCounter = 1;
            var currentTrip = new Trip { TripId = tripCounter++ };

            foreach (var delivery in sortedDeliveries)
            {
                // Edge Case: إذا كان الطرد يفيض عن السعة المتبقية في الرحلة الحالية
                if (currentTrip.TotalWeight + delivery.Weight > MaxCapacity)
                {
                    if (currentTrip.Deliveries.Any())
                        trips.Add(currentTrip);

                    currentTrip = new Trip { TripId = tripCounter++ };
                }

                currentTrip.Deliveries.Add(delivery);
            }

            if (currentTrip.Deliveries.Any())
                trips.Add(currentTrip);

            return trips;
        }
    }
}
