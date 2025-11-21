import type { AuditedEntityDto, EntityDto } from '@abp/ng.core';

export interface CityDto {
  name?: string;
  country?: string;
}

export interface CitySearchRequestDto {
  partialCityName?: string;
  resultLimit: number;
  skipCount: number;
}

export interface CoordinateDto {
  latitude: number;
  longitude: number;
}

export interface CreateUpdateDestinationDto {
  name: string;
  region: string;
  country: string;
  population: number;
  coordinate: CoordinateDto;
  photos: DestinationPhotoDto[];
}

export interface DestinationDto extends AuditedEntityDto<string> {
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
