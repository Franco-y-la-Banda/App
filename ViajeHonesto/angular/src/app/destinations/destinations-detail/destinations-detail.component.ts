import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { DestinationService } from '../../proxy/destinations/destination.service';
import {
  CityDetailsDto,
  CityDetailsSearchRequestDto,
  CoordinateDto,
} from 'src/app/proxy/destinations';
import { CoreModule, SessionStateService } from '@abp/ng.core';
import { DomSanitizer, SafeResourceUrl, SafeUrl } from '@angular/platform-browser';
import { LoadingSpinner } from 'src/app/shared/loading-spinner/loading-spinner';

@Component({
  selector: 'app-destination-detail',
  standalone: true,
  imports: [CommonModule, CoreModule, LoadingSpinner],
  templateUrl: './destinations-detail.component.html',
  styleUrls: ['./destinations-detail.component.scss'],
})
export class DestinationsDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly destinationService = inject(DestinationService);
  private sessionState = inject(SessionStateService);
  private readonly sanitizer = inject(DomSanitizer);

  destination: CityDetailsDto = null;
  request: CityDetailsSearchRequestDto = { wikiDataId: null };
  loading = true;
  language: string;
  embedMapUrl: SafeResourceUrl | null = null;
  mapUrl: SafeUrl | null = null;
  errorMessage: string | null = null;

  ngOnInit(): void {
    this.request.wikiDataId = this.route.snapshot.paramMap.get('id');

    if (this.request.wikiDataId) {
      this.loadDetails(this.request.wikiDataId);
    }

    this.language = this.sessionState.getLanguage().substring(0, 2);
  }

  private loadDetails(id: string): void {
    this.loading = true;

    this.destinationService.searchCityDetails(this.request, {skipHandleError: true}).subscribe({
      next: result => {
        this.destination = result;
        this.loading = false;

        if (this.destination.coordinate) {
          this.loadMap(this.destination.coordinate);
        }
      },
      error: err => {
        console.error('Error al cargar detalles de destino:', err);
        this.errorMessage = '::Destinations:DetailsLoadError';
        this.loading = false;
      },
    });
  }

  private loadMap(coord: CoordinateDto): void {
    this.embedMapUrl = this.getMapUrl(coord);
    this.mapUrl = this.getGoogleMapsLink(coord);
  }

  goBack(): void {
    var prevPage = window.location.href;

    window.history.go(-1);

    setTimeout(function () {
      if (window.location.href == prevPage) {
        window.location.href = window.location.origin + '/destinations';
      }
    }, 500);
  }

  getMapUrl(coords: CoordinateDto): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(
      `https://maps.google.com/maps?q=${coords.latitude},${coords.longitude}&z=12&output=embed`,
    );
  }

  getGoogleMapsLink(coords: CoordinateDto): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustUrl(
      `https://www.google.com/maps/search/?api=1&query=${coords.latitude},${coords.longitude}`,
    );
  }
}
