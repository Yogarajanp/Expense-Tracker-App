import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { ToastrService, provideToastr } from 'ngx-toastr';
import { provideCharts } from 'ng2-charts';
export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes), provideClientHydration() ,  
    provideAnimationsAsync(), 
    provideToastr(),provideCharts(),provideHttpClient(withFetch()), provideAnimationsAsync()]
};
