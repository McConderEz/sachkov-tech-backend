using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SachkovTech.Core.Converters;

public class DateTimeKindValueConverter(DateTimeKind kind, ConverterMappingHints? mappingHints = null)
    : ValueConverter<DateTime, DateTime>(
        v => v.ToUniversalTime(),
        v => DateTime.SpecifyKind(v, kind),
        mappingHints);