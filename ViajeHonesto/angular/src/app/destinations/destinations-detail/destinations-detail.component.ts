import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { DestinationService } from '../../proxy/destinations/destination.service';
import { CityDetailsDto, CityDetailsSearchRequestDto } from 'src/app/proxy/destinations';
import { CoreModule, SessionStateService } from '@abp/ng.core';

@Component({
  selector: 'app-destination-detail',
  standalone: true,
  imports: [CommonModule, CoreModule],
  templateUrl: './destinations-detail.component.html',
  styleUrls: ['./destinations-detail.component.scss'],
})
export class DestinationsDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly destinationService = inject(DestinationService);
  private sessionState = inject(SessionStateService);

  destination: CityDetailsDto = null;
  request: CityDetailsSearchRequestDto = { wikiDataId: null };
  loading = true;
  language: string;

  ngOnInit(): void {
    this.request.wikiDataId = this.route.snapshot.paramMap.get('id');

    if (this.request.wikiDataId) {
      this.loadDetails(this.request.wikiDataId);
    }
    this.language = this.sessionState.getLanguage().substring(0, 2);
  }

  private loadDetails(id: string): void {
    this.loading = true;

    this.destinationService.searchCityDetails(this.request).subscribe({
      next: result => {
        this.destination = result;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.loading = false;
      },
    });
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
}
