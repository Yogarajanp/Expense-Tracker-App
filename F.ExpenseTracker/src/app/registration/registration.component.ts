import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { RegistrationDialogComponent } from '../registration-dialog/registration-dialog.component';
import { UserService } from '../user.service';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink],
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent {
  registrationForm: FormGroup;
  
  errorMessage: string = '';
  isUsernameAvailable = true;

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private dialog: MatDialog,
    private http: HttpClient,
    private router: Router,
    private service: UserService
  ) {
    this.registrationForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required, Validators.minLength(10), Validators.pattern('^[a-zA-Z0-9]{10,}$')]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.pattern('^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$')]]
    });
  }

  async register() {
    if (this.registrationForm.invalid) {
      this.toastr.error('Please fill in the form correctly');
      return;
    }

    const { email, username, password } = this.registrationForm.value;
    const response = await this.service.GetUser(email,password);
    if (response) {
      this.toastr.error('User already exists. Try to login.');
    } else {
      this.service.RegisterPost({ Mail: email, UserName: username, Password: password })
        .subscribe(
          response => {
            console.log('Registration successful!', response);
            this.toastr.success('User Registered successfully!');
            this.router.navigate(['/login']);
          },
          error => {
            console.error('Registration error:', error);
            this.errorMessage = error.error?.message || 'An error occurred during registration.';
            this.toastr.error(this.errorMessage);
          }
        );
    }
  }

  openDialog(title: string, message: string) {
    this.dialog.open(RegistrationDialogComponent, {
      data: { title, message }
    });
  }
}
