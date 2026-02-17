import { Routes } from '@angular/router';
import { authGuard } from '@abp/ng.core';

/**
 * Rutas del módulo de Destinations
 * Utiliza lazy loading para optimizar la carga inicial de la aplicación
 * El authGuard asegura que solo usuarios autenticados puedan acceder
 */
export const DESTINATIONS_ROUTES: Routes = [
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./destinations-list/destinations-list.component').then(
        c => c.DestinationsListComponent
      ),
  },
  {
    path: ':id',
    loadComponent: () =>
      import('./destinations-detail/destinations-detail.component').then(
        c => c.DestinationsDetailComponent
      )
  }
];
