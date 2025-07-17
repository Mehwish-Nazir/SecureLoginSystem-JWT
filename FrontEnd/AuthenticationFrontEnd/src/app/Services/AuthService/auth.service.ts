import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environment/environment';
import { Login,LoginResponse } from '../../Models/Login';
import { Observable, tap, catchError,finalize, throwError } from 'rxjs';
import { TokenService } from '../TokenService/token-service.service';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loginURL=environment.loginURL;
  constructor(private http:HttpClient,
    private tokenService:TokenService
  ) { }

  Login(login:Login):Observable<LoginResponse>{
    return this.http.post<LoginResponse>(`${this.loginURL}`, login).pipe(
      tap(response=>{
       if(response.role){
        this.tokenService.setToken(response.token);
       }

       if(response.token){
        this.tokenService.setRole(response.email);
       }
      }),catchError(err=>{
               return throwError(()=>err);
      })

    );
  }

}
