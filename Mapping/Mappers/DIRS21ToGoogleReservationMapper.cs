using DynamicMappingSystem.Mapping;


namespace DynamicMappingSystem.Mapping.Mappers
{
    public class DIRS21ToGoogleReservationMapper : IMapper
    {
        public object Map(object source)
        {
            var reservation = (DIRS21.Reservation)source;
            return new Google.Reservation
            {
                Id = reservation.ReservationId.ToString(),
                GuestName = reservation.GuestFullName,
                ArrivalDate = reservation.CheckInDate,
                DepartureDate = reservation.CheckOutDate
            };
        }
    }
}
