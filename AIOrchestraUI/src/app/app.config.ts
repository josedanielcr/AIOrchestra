import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideAuth0 } from '@auth0/auth0-angular';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), 
    provideAnimationsAsync(),
    provideAuth0({
      domain: 'dev-qcwyqko8w0p3q5ht.us.auth0.com',
      clientId: 'HfmRi84AiwKBW1b8ToROcUybkCauDyTQ',
      authorizationParams: {
        redirect_uri: window.location.origin
      }
    })
  ]
};