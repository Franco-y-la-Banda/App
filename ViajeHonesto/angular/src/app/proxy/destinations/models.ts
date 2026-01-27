import type { AuditedEntityDto, EntityDto } from '@abp/ng.core';

export interface CityDetailsDto {
  wikiDataId?: string;
  name?: string;
  country?: string;
  region?: string;
  population: number;
  coordinate: CoordinateDto;
  photos: DestinationPhotoDto[];
  isSaved: boolean;
  localId?: string;
}

export interface CityDetailsSearchRequestDto {
  wikiDataId: string;
}

export interface CityDto {
  wikiDataId?: string;
  name?: string;
  country?: string;
  region?: string;
  population: number;
}

export interface CitySearchRequestDto {
  partialCityName?: string;
  resultLimit: number;
  skipCount: number;
  countryCode?: string;
  regionCode?: string;
  minPopulation?: number;
  maxPopulation?: number;
  sort?: string;
}

export interface CoordinateDto {
  latitude: number;
  longitude: number;
}

export interface CreateUpdateDestinationDto {
  wikiDataId?: string;
  name: string;
  region: string;
  country: string;
  population: number;
  coordinate: CoordinateDto;
  photos: DestinationPhotoDto[];
}

export interface DestinationDto extends AuditedEntityDto<string> {
  wikiDataId?: string;
  name?: string;
  country?: string;
  region?: string;
  population: number;
  coordinate: CoordinateDto;
  photos: DestinationPhotoDto[];
}

export interface DestinationPhotoDto extends EntityDto {
  photoId?: string;
  path?: string;
}

export interface ISOCodeDto {
  isoCode?: string;
  name?: string;
}
