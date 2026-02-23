import { Injectable } from '@angular/core';
import { CitySearchRequestDto, CityDto, ISOCodeDto } from '../../proxy/destinations/models';

export interface DestinationListState {
  searchParams: CitySearchRequestDto;
  destinations: CityDto[];
  totalCount: number;
  currentPage: number;
  hasData: boolean;
  selectedCountry: ISOCodeDto;
  selectedRegion: ISOCodeDto;
  isFilterCollapsed: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class DestinationStateService {
  private state: DestinationListState | null = null;

  setState(state: DestinationListState): void {
    this.state = { ...state, hasData: true };
  }

  getState(): DestinationListState | null {
    return this.state;
  }

  clearState(): void {
    this.state = null;
  }
}
