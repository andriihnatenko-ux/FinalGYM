using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Converters;

public class NullableDateTimeUtcConverter() : ValueConverter<DateTime?, DateTime?>(
    x => x.HasValue ? x.Value.ToUniversalTime() : x,
    x => x.HasValue
        ? x.Value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc)
            : x.Value.ToUniversalTime()
        : x);
