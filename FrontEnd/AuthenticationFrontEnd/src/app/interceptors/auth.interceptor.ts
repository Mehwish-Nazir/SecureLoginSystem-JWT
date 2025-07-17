import { HttpInterceptorFn, HttpRequest, HttpHandler, HttpEvent} from '@angular/common/http';
import { TokenService } from '../Services/TokenService/token-service.service';
import { Inject } from '@angular/core';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  /*
   step1: getToken
   step2: set Header and inject token into header
   step3: clone request
   step4: next to clone request
   step5: next that request
  */ 
const tokenService=Inject(TokenService);
const token=tokenService.getToken();
if(token){
  const clonedRequest=req.clone({
    setHeaders:{
      Authorization:`Beare ${token}`
    }
  });
  return next(clonedRequest);
}

  return next(req);
};
