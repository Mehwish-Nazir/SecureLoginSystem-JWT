import { Injectable } from '@angular/core';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class LogginServiceService {

  constructor() { }

  /*
    Spread Operator
    const user = { name: 'Ahmad', email: 'ahmad@gmail.com' };
const updated = { ...user, role: 'Admin' };
output
{ name: 'Ahmad', email: 'ahmad@gmail.com,role: 'Admin' }
  */ 


  log(message:string, ...optionalParams:any[]){
    if(!environment.production){
      console.log(`[LOG] $message`, ...optionalParams);
    }
  }

  warn(message:string, ...optionalParams:any[]){
    if(!environment.production){
      console.warn(`[WARN] $message`, ...optionalParams)
    }
  }

  error(message:string, ...optionalParams:any[]){
    console.error(`[ERROR] $message`, ...optionalParams);
  }
  
}
