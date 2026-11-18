import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { providePrimeNG } from 'primeng/config';
import { routes } from './app.routes';
import { OverloadPreset } from '../theme/overload-preset';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    providePrimeNG({
      theme: {
        preset: OverloadPreset,
        options: {
          // Dark mode is active when <html> has the "app-dark" class (see ThemeService)
          darkModeSelector: '.app-dark',
          // Keeps PrimeNG styles in a layer so global CSS wins without !important
          cssLayer: { name: 'primeng', order: 'theme, base, primeng' },
        },
      },
    }),
  ],
};
