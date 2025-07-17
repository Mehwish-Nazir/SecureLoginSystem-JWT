import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environment/environment';
import { formatDate } from '@angular/common';
import { Observable, tap, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UploadImageService {

  private uploadProfileImgURL=environment.uploadProfileImgURL;
  constructor(private http:HttpClient) { }

  uploadImage(img: File): Observable<{ imagePath: string }> {
  const formData = new FormData();   // to upload image on frontEnd formData is used an on backend IFormFile is used
  formData.append('profileImage', img); // must match the backend parameter name('profileImage') 

  return this.http.post<{ imagePath: string }>(`${this.uploadProfileImgURL}`, formData).pipe(
    tap(response => {
      console.log('Image uploaded successfully', response.imagePath);
    }),
    catchError((error: HttpErrorResponse) => {
      console.error('Image upload failed:', error.message);
      return throwError(() => error); // rethrow the error
    })
  );
}

}



