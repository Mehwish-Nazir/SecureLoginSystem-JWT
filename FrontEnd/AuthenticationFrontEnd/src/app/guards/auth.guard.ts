import { CanActivateFn, Router } from '@angular/router';
import { Inject } from '@angular/core';
import { TokenService } from '../Services/TokenService/token-service.service';
export const authGuard: CanActivateFn = (route, state) => {

  const tokenService=Inject(TokenService);
   const router=Inject(Router);

   const token=tokenService.getToken();
   if(token){
    return true;   //if found token , then navgate ti dashboard, admin panel 
   }
   else{
  router.navigate(['/login']);   /// if no token found and user try to acess dashboard , it will redirect to login page...
  return false;
   }
 
};
