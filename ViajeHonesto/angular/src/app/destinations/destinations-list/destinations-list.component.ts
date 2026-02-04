import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PagedResultDto, CoreModule } from '@abp/ng.core';
import { DestinationService } from '../../proxy/destinations/destination.service';
import { CitySearchRequestDto, CityDto } from '../../proxy/destinations/models';
import { finalize, retry } from 'rxjs/operators';
import { NgbPaginationModule, NgbCollapse } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-destinations-list',
  standalone: true,
  imports: [CommonModule, FormsModule, CoreModule, NgbPaginationModule, NgbCollapse],
  templateUrl: './destinations-list.component.html',
  styleUrls: ['./destinations-list.component.scss'],
})
export class DestinationsListComponent {
  // Inyección de dependencias usando la nueva sintaxis de inject()
  private readonly destinationService = inject(DestinationService);

  /**
   * Lista de destinos obtenidos de la API
   */
  destinations: CityDto[] = [];

  /**
   * Indica si hay una petición en curso
   */
  loading = false;

  /**
   * Indica si el usuario ya realizó una búsqueda
   */
  submitted = false;

  /**
   * Indica si el filtro está activo
   */
  isFilterCollapsed = true;

  /**
   * Parámetros de búsqueda y paginación
   * - country: Filtro por país específico
   * - skipCount: Número de registros a saltar (para paginación)
   * - resultLimit: Número máximo de registros por página
   * - partialCityName: Nombre de la ciudad
   * - countryCode: Código de 2 caractéres de un país, según ISO 3166
   * - maxPopulation: Población máxima
   * - minPopulation: Población mínima
   * - sort: Criterio de ordenamiento
   *   - Formato: ±SORT_FIELD,±SORT_FIELD
   *   - SORT_FIELD = countryCode | elevation | name | population
   */
  searchParams: CitySearchRequestDto = {
    skipCount: 0,
    resultLimit: 9,
    partialCityName: '',
    countryCode: null,
    regionCode: null,
    maxPopulation: null,
    minPopulation: null,
    sort: null,
  };

  /**
   * Total de registros disponibles (para paginación)
   */
  totalCount = 0;

  /**
   * Página actual (basada en 1)
   */
  currentPage = 1;

  /**
   * Manejo de errores
   */
  errorMessage: string | null = null;

  // ngOnInit(): void {
  //   // Cargar los destinos al inicializar el componente
  //   this.loadDestinations();
  // }

  /**
   * Carga los destinos desde la API
   */
  private loadDestinations(): void {
    this.loading = true;

    this.destinationService
      .searchCities(this.searchParams, {
        skipHandleError: true,
      })
      .pipe(
        retry({ count: 1, delay: 1000 }),
        finalize(() => {
          this.loading = false;
        }),
      )
      .subscribe({
        next: (result: PagedResultDto<CityDto>) => {
          // Asignar los resultados al array de destinos
          this.destinations = result.items || [];
          this.totalCount = result.totalCount || 0;
          this.errorMessage = null;
        },
        error: error => {
          // Manejar errores de la API
          console.error('Error al cargar destinos:', error);
          this.errorMessage = '::Destinations:LoadError';
          this.destinations = [];
          this.totalCount = 0;
        },
      });
  }

  /**
   * Maneja el evento de búsqueda
   * Reinicia la paginación y recarga los datos
   */
  onSearch(): void {
    if (!this.loading) {
      this.submitted = true;

      // Sin errores previos
      this.errorMessage = null;

      // Reiniciar a la primera página
      this.searchParams.skipCount = 0;
      this.currentPage = 1;

      // Recargar los datos
      this.loadDestinations();
    }
  }

  /**
   * Limpia los filtros de búsqueda y recarga todos los destinos
   */
  clearSearch(): void {
    this.searchParams.partialCityName = '';
    this.errorMessage = null;
    this.destinations = [];
    this.totalCount = 0;
    this.submitted = false;
  }

  /**
   * Maneja el cambio de página
   *
   * @param page - Número de la nueva página (basada en 1)
   */
  onPageChange(page: number): void {
    this.currentPage = page;
    // Calcular el skipCount basado en la página actual
    this.searchParams.skipCount = (page - 1) * this.searchParams.resultLimit;
    this.loadDestinations();
  }
}
