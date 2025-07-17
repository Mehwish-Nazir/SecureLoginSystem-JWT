import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';

import { ApiResponse } from '../../../Models/ApiResponse';
import { RegistrationResponse } from '../../../Models/RegisterUser';
import { RegisterUserService } from '../../../Services/Registration/register-user.service';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-registration',
  templateUrl: './user-registration.component.html',
  styleUrls: ['./user-registration.component.css'],
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule, MatSnackBarModule] //  Add here
})
export class UserRegistrationComponent implements OnInit {
  selectedFile: File | null = null;
  registrationForm!: FormGroup;
  isSubmitting = false;

  constructor(
    private registrationService: RegisterUserService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar, //  Inject snackbar
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registrationForm = this.fb.group(
      {
        username: ['', [Validators.required, Validators.maxLength(255)]],
        email: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
        phoneNumber: ['', [Validators.required, Validators.pattern(/^(\+92|0)?3\d{2}\d{7}$/)]],
        password: ['', [Validators.required]],
        confirmPassword: ['', [Validators.required]],
        profileImagePath: ['']
      },
      {
        validators: this.passwordMatchValidator
      }
    );
  }

  passwordMatchValidator(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  onImageSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onSubmit(): void {
    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    const formData = new FormData();

    Object.entries(this.registrationForm.value).forEach(([key, value]) => {
      if (key !== 'profileImagePath' && value !== null && value !== undefined) {
        formData.append(key, value.toString());
      }
    });

    if (this.selectedFile) {
      formData.append('ProfilePicture', this.selectedFile);
    }

    this.registrationService.registerUser(formData).subscribe({
      next: (res: ApiResponse<RegistrationResponse>) => {
        this.snackBar.open(res.message, 'Close', {
          duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'center',
        });
        // this.router.navigate(['/login']); // Optional redirect
      },
      error: (err: HttpErrorResponse) => {
  const msg = err.error?.message || 'Registration failed.';
  this.snackBar.open(msg, 'Close', {
    duration: 4000,
    panelClass: ['mat-warn'],
    verticalPosition: 'top',
    horizontalPosition: 'center',
  });
  console.error('Registration error:', err);
      },
      complete: () => {
        this.isSubmitting = false;
      }
    });
  }
}
