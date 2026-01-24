using System;
using System.Collections.Generic;
using System.Linq;
using Elders.Iso3166;
using Microsoft.AspNetCore.Mvc;
namespace ViajeHonesto.Destinations
{
    public class ISOCodeLookupService : ViajeHonestoAppService, IISOCodeLookupService
    {
        [HttpGet("api/app/iso-code-lookup/countries")]
        public List<ISOCodeDto> GetCountries()
        {
            var countries = Country.GetAllCountries();

            return countries.Select(c => new ISOCodeDto
            {
                ISOCode = c.TwoLetterCode,
                Name = c.Name
            }).ToList();
        }

        [HttpGet("api/app/iso-code-lookup/regions")]
        public List<ISOCodeDto> GetRegions(string CountryCode)
        {
            var regions = new Country(CountryCode).Subdivisions;
            return regions.Select(r => new ISOCodeDto
            {
                ISOCode = r.Code[3..],
                Name = r.Name
            }).ToList();
        }
    }
}
