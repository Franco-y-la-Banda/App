using System.Collections.Generic;

namespace ViajeHonesto.Destinations
{
    public interface IISOCodeLookupService
    {
        public List<ISOCodeDto> GetCountries();
        public List<ISOCodeDto> GetRegions(string CountryCode);
    }
}
