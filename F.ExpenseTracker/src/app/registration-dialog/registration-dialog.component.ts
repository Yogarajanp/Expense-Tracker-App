
import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-registration-dialog',
  standalone: true,
  imports: [],
  templateUrl: './registration-dialog.component.html',
  styleUrl: './registration-dialog.component.scss'
})
export class RegistrationDialogComponent {
  titleColor = 'blue';
  titleFontSize = '20px';
  contentFontSize = '16px';
  buttonColor = '#3f51b5';

  constructor(
    private dialogRef: MatDialogRef<RegistrationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { title: string, message: string }
  ) { }

  closeDialog() {
    this.dialogRef.close();
  }
}
