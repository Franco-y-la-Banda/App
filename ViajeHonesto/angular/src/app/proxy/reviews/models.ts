import type { AuditedEntityDto } from '@abp/ng.core';

export interface CommentDto {
  comment?: string;
}

export interface CreateReviewDto {
  destinationId: string;
  rating: RatingDto;
  comment: CommentDto;
}

export interface RatingDto {
  value: number;
}

export interface ReviewDto extends AuditedEntityDto {
  userId: string;
  destinationId: string;
  rating: RatingDto;
  comment: CommentDto;
}

export interface ReviewKey {
  destinationId?: string;
  userId?: string;
}

export interface UpdateReviewDto {
  rating: RatingDto;
  comment: CommentDto;
  lastModificationTime?: string;
}
