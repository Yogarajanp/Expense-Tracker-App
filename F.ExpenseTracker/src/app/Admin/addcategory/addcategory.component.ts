import { CommonModule } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CategoryService } from '../../category.service';

@Component({
  selector: 'app-addcategory',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './addcategory.component.html',
  styleUrls: ['./addcategory.component.css']
})
export class AddcategoryComponent implements OnInit {
  categoryForm: FormGroup;
  @Output() categoryAdded = new EventEmitter<void>();

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddcategoryComponent>,
    private categoryService: CategoryService,
    private toastr: ToastrService
  ) {
    this.categoryForm = this.fb.group({
      categoryType: ['', Validators.required]
    });
  }

  ngOnInit(): void { }

  onSubmit() {
    if (this.categoryForm.invalid) {
      return;
    }

    const category = {
      categoryID: 0,
      categoryType: this.categoryForm.value.categoryType,
      deleteID: 1
    };

    this.categoryService.insertCategory(category).subscribe(
      event => {
        if (event) {
          this.categoryAdded.emit();
          this.dialogRef.close(event);
          this.toastr.success('Category added successfully!');
        }
      },
      error => {
        console.error('Error adding category:', error);
        this.toastr.error('Failed to add category!');
      }
    );
  }
}
