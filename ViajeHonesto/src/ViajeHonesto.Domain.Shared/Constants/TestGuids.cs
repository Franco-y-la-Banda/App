using System;

namespace ViajeHonesto.Constants;
public static class TestGuids
{
    public static Guid DestinationGuid(int index)
    {
        var hex = index.ToString("X8").PadLeft(12, '0');
        var guidString = $"11111111-1111-1111-1111-{hex}";
        return Guid.Parse(guidString);
    }

    public static Guid UserGuid(int index)
    {
        var hex = index.ToString("X8").PadLeft(12, '0');
        var guidString = $"22222222-2222-2222-2222-{hex}";
        return Guid.Parse(guidString);
    }
}