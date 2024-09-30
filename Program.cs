using DynamicMappingSystem.Mapping.Mappers;
using System;

class Program
{
    static void Main(string[] args)
    {
        MapHandler mapHandler = new MapHandler();

        // Create dummy data for DIRS21 reservation
        var dirReservation = new DIRS21.Reservation
        {
            ReservationId = 789,
            GuestFullName = "Mladen Raguž",
            CheckInDate = DateTime.Now,
            CheckOutDate = DateTime.Now.AddDays(2)
        };

        // Map DIRS21 reservation to Google reservation
        var googleReservation = mapHandler.Map(dirReservation, "DIRS21.Reservation", "Google.Reservation");
        Console.WriteLine($"Mapped to Google.Reservation: {googleReservation}");
    }
}
