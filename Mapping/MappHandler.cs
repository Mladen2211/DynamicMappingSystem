using DynamicMappingSystem.Mapping.Mappers;

namespace DynamicMappingSystem.Mapping.Mappers
{
    public class MapHandler
    {
        private readonly Dictionary<string, IMapper> _mappers = new Dictionary<string, IMapper>();

        public MapHandler()
        {
            // Register existing mappers
            _mappers.Add("DIRS21.Reservation->Google.Reservation", new DIRS21ToGoogleReservationMapper());
            _mappers.Add("Google.Reservation->DIRS21.Reservation", new GoogleToDIRS21ReservationMapper());

        }

        public object Map(object data, string sourceType, string targetType)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data cannot be null.");
            }

            string key = $"{sourceType}->{targetType}";
            if (_mappers.ContainsKey(key))
            {
                return _mappers[key].Map(data);
            }
            else
            {
                throw new NotSupportedException($"Mapping from {sourceType} to {targetType} is not supported.");
            }
        }
    }
}
