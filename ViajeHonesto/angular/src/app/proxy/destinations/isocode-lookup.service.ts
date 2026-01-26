import type { ISOCodeDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ISOCodeLookupService {
  apiName = 'Default';
  

  getCountries = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ISOCodeDto[]>({
      method: 'GET',
      url: '/api/app/iso-code-lookup/countries',
    },
    { apiName: this.apiName,...config });
  

  getRegionsByCountryCode = (CountryCode: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ISOCodeDto[]>({
      method: 'GET',
      url: '/api/app/iso-code-lookup/regions',
      params: { countryCode: CountryCode },
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
