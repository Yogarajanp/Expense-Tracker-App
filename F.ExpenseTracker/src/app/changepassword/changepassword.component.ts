import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ToastrService } from 'ngx-toastr'; // Assuming you're using ngx-toastr
import { UserService } from '../user.service';

@Component({
  selector: 'app-changepassword',
  standalone: true,
  imports: [MatFormFieldModule, ReactiveFormsModule, CommonModule, MatInputModule],
  templateUrl: './changepassword.component.html',
  styleUrls: ['./changepassword.component.css']
})
export class ChangepasswordComponent {
  changePasswordForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<ChangepasswordComponent>,
    @Inject(MAT_DIALOG_DATA) public data?: any // Optional data for the dialog
  ) {
    this.changePasswordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    });
  }

  passwordMatchValidator(formGroup: FormGroup) {
    const newPassword = formGroup.get('newPassword')?.value || '';
    const confirmPassword = formGroup.get('confirmPassword')?.value || '';
    return newPassword === confirmPassword ? null : { mismatch: true };
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }

  onSubmit() {
    if (this.changePasswordForm.valid) {
      const { currentPassword, newPassword } = this.changePasswordForm.value;

      // Implement your password change logic here with UserService
      this.userService.changePassword(newPassword)
        .subscribe({
          next: response => {
            console.log('Password changed successfully', response);
            this.dialogRef.close(response);
            this.toastr.success("Password changed successfully");
          },
          error: error => {
            console.error('Error changing password', error);
            this.toastr.error("Error changing password"); // Display error message
          }
        });
    }
  }
}
