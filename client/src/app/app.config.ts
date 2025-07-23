import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';

import { routes } from './app.routes';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';
import { InitService } from '../core/services/init-service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes,withViewTransitions()),
    provideHttpClient(),
    provideAppInitializer(async () => {

const initService = inject(InitService);

return new Promise<void>((resolve) => {
  setTimeout(async () => {
try {
  return lastValueFrom(initService.init());
} finally  {
     const splash = document.getElementById('initial-splash');
if (splash) {
      splash.remove();
     }
     resolve()
}
  },500)
})



    }
  )
]
};
