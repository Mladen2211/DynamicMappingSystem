using DynamicMappingSystem.Mapping;

namespace DynamicMappingSystem.Mapping.Mappers
{
    public class GoogleToDIRS21ReservationMapper : IMapper
    {
        public object Map(object source)
        {
            var reservation = (Google.Reservation)source;
            return new DIRS21.Reservation
            {
                ReservationId = int.Parse(reservation.Id),
                GuestFullName = reservation.GuestName,
                CheckInDate = reservation.ArrivalDate,
                CheckOutDate = reservation.DepartureDate
            };
        }
    }
}
