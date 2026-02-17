import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { DestinationService } from '../../proxy/destinations/destination.service';
import { CityDetailsDto, CityDetailsSearchRequestDto } from 'src/app/proxy/destinations';

@Component({
  selector: 'app-destination-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './destinations-detail.component.html',
  styleUrls: ['./destinations-detail.component.scss'],
})

export class DestinationsDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly destinationService = inject(DestinationService);

  destination: CityDetailsDto = null;
  request: CityDetailsSearchRequestDto = {wikiDataId: null};
  loading = true;

  ngOnInit(): void {
    this.request.wikiDataId = this.route.snapshot.paramMap.get('id');

    if (this.request.wikiDataId) {
      this.loadDetails(this.request.wikiDataId);
    }
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
}
