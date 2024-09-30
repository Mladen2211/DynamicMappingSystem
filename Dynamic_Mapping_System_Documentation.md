
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
## Assumptions

### Consistent Data Structures:
It is assumed that the internal DIRS21 models and partner models have a predictable and consistent structure. Any changes to the model structure should be communicated and managed properly.

### Basic Type Compatibility:
The implementation assumes that the types being mapped (e.g., integers, strings, dates) are compatible between the source and target models. Complex types or structures may require additional mapping logic.

### Fixed Mapping Requirements:
The mapping logic is based on predefined rules for converting data. It assumes that these rules are stable and do not change frequently. If business rules change, the mappers may need to be updated accordingly.

### Error Handling Expectations:
It is assumed that the user will handle exceptions appropriately. The system throws exceptions for unsupported mappings or invalid data, and it is assumed that the application using this system will implement adequate error handling.

### Single Source and Target Types:
The implementation assumes that each mapping operation involves a single source type to a single target type. Complex scenarios involving multiple sources or targets may require additional logic.

## Potential Limitations

### Performance:
Depending on the size of the data being mapped and the complexity of the mappings, performance could become an issue. The system may need optimizations for large-scale data transformations.

### Limited Flexibility:
While the system is designed to be extensible, the initial implementation may not support all edge cases or complex mapping scenarios. Custom mappings might require significant development effort.

### Error Propagation:
If there are errors in the source data, the system may throw exceptions during mapping without gracefully handling the issue. This could lead to application crashes or unexpected behavior unless properly managed.

### Versioning and Backward Compatibility:
Changes to internal or partner model structures could lead to versioning issues. The system may need to handle backward compatibility for older models, which could complicate maintenance.

### Lack of Validation Logic:
The current implementation does not include extensive validation logic for the data being mapped. If the source data does not conform to expected formats, errors may occur during the mapping process.
## Approximate development time 
To create and test this solutione together with the wruiting of this documentattion took approximately 2.5 hours
## Conclusion

The system is flexible and allows easy expansion by adding new models and mapping rules. By following the principles of clean code and extensibility, the mapping system is scalable for future use cases involving different partner models.