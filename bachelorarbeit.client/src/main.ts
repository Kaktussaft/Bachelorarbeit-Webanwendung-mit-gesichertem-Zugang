import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter, Router } from '@angular/router';
import { routes } from './app/app.routes';
import { provideHttpClient } from '@angular/common/http';
import { AuthService } from './app/services/authentication-service.service';

function initializeApp(
  authService: AuthService,
  router: Router
): () => Promise<void> {
  return () =>
    new Promise<void>((resolve) => {
      authService.checkInitialAuthtentication().then((isAuthenticated) => {
        if (!isAuthenticated) {
          router.navigate(['/login']);
        }
        resolve();
      });
    });
}

bootstrapApplication(AppComponent, {
  providers: [provideRouter(routes), provideHttpClient()],
}).catch((err) => console.error(err));
