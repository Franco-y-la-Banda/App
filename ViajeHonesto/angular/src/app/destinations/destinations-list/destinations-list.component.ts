import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PagedResultDto, CoreModule } from '@abp/ng.core';
import { DestinationService } from '../../proxy/destinations/destination.service';
import { CitySearchRequestDto, CityDto } from '../../proxy/destinations/models';
import { debounceTime, distinctUntilChanged, finalize, map, retry } from 'rxjs/operators';
import { NgbPaginationModule, NgbCollapse } from '@ng-bootstrap/ng-bootstrap';
import { ISOCodeDto } from '../../proxy/destinations/models';
import { ISOCodeLookupService } from 'src/app/proxy/destinations';
import { merge, Observable, OperatorFunction, Subject } from 'rxjs';
import { NgbTypeahead } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-destinations-list',
  standalone: true,
  imports: [CommonModule, FormsModule, CoreModule, NgbPaginationModule, NgbCollapse, NgbTypeahead],
  templateUrl: './destinations-list.component.html',
  styleUrls: ['./destinations-list.component.scss'],
})
export class DestinationsListComponent implements OnInit {
  // Inyección de dependencias usando la nueva sintaxis de inject()
  private readonly destinationService = inject(DestinationService);
  private readonly isoCodeLookupService = inject(ISOCodeLookupService);

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
  searchParams = this.defaultSearchParams;

  private get defaultSearchParams(): CitySearchRequestDto {
    return {
      skipCount: 0,
      resultLimit: 9,
      partialCityName: '',
      countryCode: null,
      regionCode: null,
      minPopulation: null,
      maxPopulation: null,
      sort: null,
    };
  }

  allCountries: ISOCodeDto[] = [];
  allRegionsForCountry: ISOCodeDto[] = [];

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

  /**
   * País seleccionado en el filtro
   */
  selectedCountry: ISOCodeDto = null;

  /**
   * Región seleccionada en el filtro
   */
  selectedRegion: ISOCodeDto = null;

  /**
   * Necesario para mostrar la lista de países al hacer click, ni idea que hace
   */
  countryFocus$ = new Subject<string>();
  countryClick$ = new Subject<string>();

  regionFocus$ = new Subject<string>();
  regionClick$ = new Subject<string>();

  ngOnInit(): void {
    this.isoCodeLookupService
      .getCountries()
      .pipe()
      .subscribe({
        next: (result: ISOCodeDto[]) => {
          this.allCountries = result;
        },
      });
  }

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
    this.searchParams = this.defaultSearchParams;

    this.selectedCountry = null;
    this.selectedRegion = null;

    this.resetGridState();
  }

  private resetGridState(): void {
    this.errorMessage = null;
    this.destinations = [];
    this.totalCount = 0;
    this.submitted = false;
    this.loading = false;
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

  /**
   * Devuelve solo el nombre de un ISOCodeDto
   */
  isoNameFormatter = (x: ISOCodeDto) => x.name;

  /**
   * Filtra todos los países según lo que se escriba
   */
  searchCountries: OperatorFunction<string, readonly ISOCodeDto[]> = (
    text$: Observable<string>,
  ) => {
    const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
    const inputEvents$ = merge(this.countryFocus$, this.countryClick$).pipe(map(() => ''));

    return merge(debouncedText$, inputEvents$).pipe(
      map(term =>
        term === ''
          ? this.allCountries
          : this.allCountries.filter(v => v.name.toLowerCase().indexOf(term.toLowerCase()) > -1),
      ),
    );
  };

  searchRegions: OperatorFunction<string, readonly ISOCodeDto[]> = (text$: Observable<string>) => {
    const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
    const inputEvents$ = merge(this.regionFocus$, this.regionClick$).pipe(map(() => ''));

    return merge(debouncedText$, inputEvents$).pipe(
      map(term =>
        term === ''
          ? this.allRegionsForCountry
          : this.allRegionsForCountry.filter(
              v => v.name.toLowerCase().indexOf(term.toLowerCase()) > -1,
            ),
      ),
    );
  };

  /**
   * Evento al seleccionar país
   */
  onCountrySelect(event: any) {
    const country = event.item as ISOCodeDto;
    this.searchParams.countryCode = country.isoCode;

    this.searchParams.regionCode = null;
    this.selectedRegion = null;
    this.isoCodeLookupService.getRegionsByCountryCode(country.isoCode).subscribe({
      next: (result: ISOCodeDto[]) => {
        this.allRegionsForCountry = result;
      },
    });
  }

  /**
   * Evento al seleccionar región
   */
  onRegionSelect(event: any) {
    const region = event.item as ISOCodeDto;
    this.searchParams.regionCode = region.isoCode;
  }

  /**
   * Evento al borrar el input de país manualmente
   */
  checkCountryClear(): void {
    if (!this.selectedCountry) {
      this.searchParams.countryCode = null;
      this.searchParams.regionCode = null;
      this.selectedCountry = null;
      this.selectedRegion = null;
    }
  }

  /**
   * Evento al borrar el input de país manualmente
   */
  checkRegionClear(): void {
    if (!this.selectedRegion && this.searchParams.countryCode) {
      this.searchParams.regionCode = null;
    }
  }

  /**
   * Valida el filtrado por población
   */
  get isPopulationValid(): boolean {
    const { minPopulation, maxPopulation } = this.searchParams;

    if (minPopulation < 0 || maxPopulation < 0) {
      return false;
    }

    if (minPopulation != null && maxPopulation != null) {
      return minPopulation <= maxPopulation;
    }
    return true;
  }
}
