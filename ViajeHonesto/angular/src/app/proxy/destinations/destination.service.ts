import type { CityDetailsDto, CityDetailsSearchRequestDto, CityDto, CityRegionSearchRequestDto, CitySearchRequestDto, CreateUpdateDestinationDto, DestinationDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DestinationService {
  apiName = 'Default';
  

  create = (input: CreateUpdateDestinationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DestinationDto>({
      method: 'POST',
      url: '/api/app/destination',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/destination/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DestinationDto>({
      method: 'GET',
      url: `/api/app/destination/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<DestinationDto>>({
      method: 'GET',
      url: '/api/app/destination',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getWithDetails = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DestinationDto>({
      method: 'GET',
      url: `/api/app/destination/${id}/with-details`,
    },
    { apiName: this.apiName,...config });
  

  searchCitiesByName = (request: CitySearchRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CityDto>>({
      method: 'GET',
      url: '/api/app/destination/search-cities-by-name',
      params: { partialCityName: request.partialCityName, resultLimit: request.resultLimit, skipCount: request.skipCount, countryCode: request.countryCode, minPopulation: request.minPopulation, maxPopulation: request.maxPopulation, sort: request.sort },
    },
    { apiName: this.apiName,...config });
  

  searchCitiesByRegion = (request: CityRegionSearchRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CityDto>>({
      method: 'GET',
      url: '/api/app/destination/search-cities-by-region',
      params: { countryCode: request.countryCode, regionCode: request.regionCode, partialCityName: request.partialCityName, resultLimit: request.resultLimit, skipCount: request.skipCount, minPopulation: request.minPopulation, maxPopulation: request.maxPopulation, sort: request.sort },
    },
    { apiName: this.apiName,...config });
  

  searchCityDetails = (request: CityDetailsSearchRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CityDetailsDto>({
      method: 'GET',
      url: '/api/app/destination/search-city-details',
      params: { wikiDataId: request.wikiDataId },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateDestinationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DestinationDto>({
      method: 'PUT',
      url: `/api/app/destination/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
