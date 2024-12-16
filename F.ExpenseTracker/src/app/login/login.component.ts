import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../user.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, MatInputModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {

  loginForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private service: UserService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  async login() {
    if (this.loginForm.invalid) {
      this.toastr.error('Please fill in the form correctly.');
      return;
    }

    const formValue = this.loginForm.value;

    try {
      const response = await this.service.GetUser(formValue.email,formValue.password);
      console.log(response);

      if (response) {
        if (response.user.password === formValue.password) {
          console.log('Login successful');
          localStorage.setItem('token', response.token);
          sessionStorage.setItem('currentUser', JSON.stringify(response));
          this.router.navigate(['/index']);
        } else {
          console.log('Incorrect password.Please try again.');
          this.toastr.error('Incorrect password. Please try again.');
        }
      }
      else {
        console.log('User not found. Please check the email.');
        this.toastr.error('User not found.');
      }
    } catch (error) {
      console.error('Login error:', error);
      this.toastr.error('An error occurred during login. Please try again.');
    }
  }
}
