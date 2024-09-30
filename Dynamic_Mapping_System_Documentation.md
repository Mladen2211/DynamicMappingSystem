
# Dynamic Mapping System Documentation



## Overview

The Dynamic Mapping System is designed to handle data transformations between internal DIRS21 models and external partner-specific models using .NET. The system supports converting DIRS21 models into partner formats and vice versa, with an emphasis on scalability, extensibility, and maintainability.
## Core Features
### Mapping System Core
The core of the system is the MapHandler class. It dynamically handles mappings between internal and external models based on the provided sourceType and targetType.

The core method signature is:
```markdown
    object Map(object data, string sourceType, string targetType);
```

    - data: The data object to be mapped.
    - sourceType: The type of the source model (e.g., "Model.Reservation").
    - targetType: The type of the target model (e.g., "Google.Reservation").

The MapHandler determines which mapping logic to apply based on the source and target model types.

### Supported Models
#### DIRS21 Reservation Model
The internal reservation model is used by DIRS21:

```
namespace DIRS21
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public string GuestFullName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}

```

#### Google Reservation Model
The external reservation model used by Google:

```
namespace Google
{
    public class Reservation
    {
        public string Id { get; set; }
        public string GuestName { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}

```

### Mapping Logic

The system uses specific mappers to convert between models. Each mapping logic is implemented in a class that implements the IMapper interface.

#### DIRS21 → Google Reservation Mapper

```
public class DirToGoogleReservationMapper : IMapper
{
    public object Map(object source)
    {
        var reservation = (Model.Reservation)source;
        return new Google.Reservation
        {
            Id = reservation.ReservationId.ToString(),
            GuestName = reservation.GuestFullName,
            ArrivalDate = reservation.CheckInDate,
            DepartureDate = reservation.CheckOutDate
        };
    }
}
```

#### Google → DIRS21 Reservation Mapper

```
public class GoogleToDirReservationMapper : IMapper
{
    public object Map(object source)
    {
        var reservation = (Google.Reservation)source;
        return new Model.Reservation
        {
            ReservationId = int.Parse(reservation.Id),
            GuestFullName = reservation.GuestName,
            CheckInDate = reservation.ArrivalDate,
            CheckOutDate = reservation.DepartureDate
        };
    }
}
```

### Error Handling
The system throws meaningful exceptions when data or mapping types are invalid.
For unsupported mappings, the system raises a NotSupportedException.
## Extensibility
The system is designed to be easily extendable. Adding support for new models involves:

    1. Creating a new model: Define the data model for the new partner.
    2. Implementing mappers: Create two mappers—one for converting from the DIRS21 model to the new model, and one for the reverse mapping.
    3. Registering the mappers: Add the new mappers to the MapHandler.

### Example of Expansion (Booking.com Model)
Here’s how you could add support for a Booking.com reservation model as an extension:

    - Create the Booking.Reservation class.
    - Implement DirToBookingReservationMapper and BookingToDirReservationMapper.
    - Register the mappers in the MapHandler constructor.
## Usage Example

Here’s a basic example of how to use the mapping system:

```
// Create a DIRS21 reservation
var dirReservation = new Model.Reservation
{
    ReservationId = 123,
    GuestFullName = "John Doe",
    CheckInDate = DateTime.Now,
    CheckOutDate = DateTime.Now.AddDays(3)
};

// Create an instance of the MapHandler
var mapHandler = new MapHandler();

// Map DIRS21 reservation to Google reservation
var googleReservation = mapHandler.Map(dirReservation, "Model.Reservation", "Google.Reservation");
Console.WriteLine($"Mapped to Google Reservation: {googleReservation}");

// Map Google reservation back to DIRS21 reservation
var dirMappedReservation = mapHandler.Map(googleReservation, "Google.Reservation", "Model.Reservation");
Console.WriteLine($"Mapped back to DIRS21 Reservation: {dirMappedReservation}");
```
## Conclusion

The system is flexible and allows easy expansion by adding new models and mapping rules. By following the principles of clean code and extensibility, the mapping system is scalable for future use cases involving different partner models.