import { Injectable } from '@angular/core';
import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, timeout } from 'rxjs/operators';
import { HttpErrorNotifierService } from './http-error-notifier.service';
import { AuthService } from 'app/core/auth/services/auth.service';

@Injectable()
export class ServerValidationErrorInterceptor implements HttpInterceptor {

    readonly requestTimeout = 60000;

    constructor(protected httpErrorHandler: HttpErrorNotifierService,
                protected authService: AuthService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        console.log("test");
        return next.handle(req)
            .pipe(
                timeout(this.requestTimeout),
                catchError(err => {
                  if (err.name == 'TimeoutError') {
                        err = new HttpErrorResponse({
                            status: 408,
                            error: 'Request timed out.'
                        });
                    }

                    if (err instanceof HttpErrorResponse) {

                      if(err.status === 200)
                        return;
                      if (err.status == 401 && this.authService.isSignedIn) {
                          this.authService.signOut();
                      }
                      else {
                          this.httpErrorHandler.handle(err, req);
                      }
                    }

                    return throwError(err);
                })
            );
    }
}
