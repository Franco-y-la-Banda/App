import { Component, Input } from '@angular/core';
import { CoreModule } from '@abp/ng.core';

@Component({
  selector: 'app-loading-spinner',
  imports: [CoreModule],
  template: `
    @if (loading) {
      <div
        id="loading-spinner"
        class="position-absolute top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center bg-white bg-opacity-75 rounded"
      >
        <div class="spinner-border text-primary" role="status">
          <span class="sr-only">{{ '::Loading' | abpLocalization }}</span>
        </div>
      </div>
    }
  `,
  styleUrl: './loading-spinner.scss',
})
export class LoadingSpinner {
  @Input() loading = false;
}
