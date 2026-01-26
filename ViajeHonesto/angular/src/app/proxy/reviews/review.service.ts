import type { CreateReviewDto, ReviewDto, ReviewKey, UpdateReviewDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  apiName = 'Default';
  

  create = (input: CreateReviewDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ReviewDto>({
      method: 'POST',
      url: '/api/app/review',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: ReviewKey, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/review/${id.destinationId}/${id.userId}`,
    },
    { apiName: this.apiName,...config });
  

  delete = (destinationId: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: '/api/app/review',
      params: { destinationId },
    },
    { apiName: this.apiName,...config });
  

  get = (id: ReviewKey, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ReviewDto>({
      method: 'GET',
      url: `/api/app/review/${id.destinationId}/${id.userId}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ReviewDto>>({
      method: 'GET',
      url: '/api/app/review',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: ReviewKey, input: UpdateReviewDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ReviewDto>({
      method: 'PUT',
      url: `/api/app/review/${id.destinationId}/${id.userId}`,
      body: input,
    },
    { apiName: this.apiName,...config });
  

  update = (destinationId: string, input: UpdateReviewDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ReviewDto>({
      method: 'PUT',
      url: '/api/app/review',
      params: { destinationId },
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
