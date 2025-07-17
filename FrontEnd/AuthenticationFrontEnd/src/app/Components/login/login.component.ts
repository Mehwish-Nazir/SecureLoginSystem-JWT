import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../Services/AuthService/auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { LogginServiceService } from '../../Services/LoggingService/logging-service.service';
import { MatSnackBar, MatSnackBarModule , } from '@angular/material/snack-bar';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, MatSnackBarModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private logger: LogginServiceService,
    private snackBar: MatSnackBar //  Corrected injection
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const loginData = this.loginForm.value;

    this.authService.Login(loginData).subscribe({
      next: (res) => {
        this.logger.log('Login successful', res);
        this.snackBar.open(
          `Welcome ${res.username}, you have successfully logged in!`,
          'Close',
          { duration: 3000,
          verticalPosition: 'top',
          horizontalPosition: 'center'
           }
        );
      },
      error: (err) => {
        this.logger.error('Login failed', err);
        this.snackBar.open(
          err.error?.message || 'Login failed. Please try again.',
          'Close',
          { duration: 3000 ,
            verticalPosition: 'top',
          horizontalPosition: 'center'
          }
        );
      }
    });
  }
}
