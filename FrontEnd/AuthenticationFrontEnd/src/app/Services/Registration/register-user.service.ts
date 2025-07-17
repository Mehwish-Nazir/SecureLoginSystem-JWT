import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environment/environment';

import { Observable, tap, catchError, finalize, throwError } from 'rxjs';

import { RegistrationResponse } from '../../Models/RegisterUser';
import { ApiResponse } from '../../Models/ApiResponse';
import { TokenService } from '../TokenService/token-service.service';

@Injectable({
  providedIn: 'root'
})
export class RegisterUserService {

  private registerUserURL = environment.registerUserURL;

  constructor(private http: HttpClient

  ) {}

  /**
   *  Register a new user with profile picture.
   * Uses multipart/form-data (FormData) to send form fields + file.
   * Expects: ApiResponse<RegistrationResponse> from backend
   */
  registerUser(data: FormData): Observable<ApiResponse<RegistrationResponse>> {
    return this.http.post<ApiResponse<RegistrationResponse>>(this.registerUserURL, data).pipe(
      tap(response => {
        console.log(
          `${response.data.username}, you have successfully registered with ID '${response.data.userID}'`,
          response
        );
      }),
      catchError(error => {
        console.error('Registration failed:', error);
        return throwError(() => new Error('Failed to register user. Please try again.'));
      }),
      finalize(() => {
        console.log(" Registration request completed.");
      })
    );
  }
}
