import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app/app.routes';
import { ToastrModule } from 'ngx-toastr';
import { importProvidersFrom } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

bootstrapApplication(AppComponent, {
  /*
  
  appConfig is a custom configuration object that typically includes global Angular providers for the application — for example:
appConfig is a custom configuration object that typically includes global Angular providers for your application — for example:

  */
  ...appConfig,
  providers:[
    provideRouter(routes),
    provideHttpClient(),
     importProvidersFrom(
      BrowserAnimationsModule,
      ToastrModule.forRoot({
        timeOut: 3000,
        positionClass: 'toast-top-right',
        preventDuplicates: true,
      })
    )
  ]
})
  .catch((err) => console.error(err));
