import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ExpenseService } from '../../expense.service';
import { HttpClient, HttpEventType, HttpHeaders, HttpResponse } from '@angular/common/http';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../category.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';


@Component({
  selector: 'app-add-expense',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './add-expense.component.html',
  styleUrl: './add-expense.component.css'
})
export class AddExpenseComponent {
  expense: any = {
    dateofTransaction: new Date(),
    description: '',
    deleteID: 0,
    userId: 1,
    amount: 0,
    modeOfTransaction: '',

    receiptPath: 'dvhgnjc',
    categoryId: ''
  };
  @Output() expenseAdded = new EventEmitter<void>();
  categories: any[] = [];
  expenseForm: FormGroup;
  selectedFile: any;
  uploadProgress: any;


  constructor(private router: Router, private fb: FormBuilder, private dialogRef: MatDialogRef<AddExpenseComponent>, private Service: ExpenseService, private categoryService: CategoryService, private http: HttpClient, public dialog: MatDialog, private toastr: ToastrService) {

    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      const userstorage = JSON.parse(currentUser);

      this.expense.userId = userstorage.user.userID;

    }
    this.expenseForm = this.fb.group({
      description: ['', [Validators.required, Validators.minLength(8)]],
      amount: ['', Validators.required, Validators.min(0)],
      modeOfTransaction: ['', Validators.required],
      category: ['', Validators.required]

    });

  }

  ngOnInit(): void {
    this.loadCategories();

  }

  onFileChange(event: any): void {
    this.selectedFile = event.target.files[0];
  }



  async loadCategories() {

    this.categoryService.getAllCategories()
      .subscribe(
        data => {

          this.categories = data;

        },
        error => {

          console.error('data fetch error:', error);

        }
      );
  }

  onSubmit() {




    const formData = new FormData();

    //formData.append('expenseDetails', JSON.stringify(this.expense));
    formData.append('expensedisc', this.expense.description);
    formData.append('expenseamt', this.expense.amount);
    formData.append('expensedate', this.expense.dateofTransaction);

    formData.append('expensemode', this.expense.modeOfTransaction);

    formData.append('expensecat', this.expense.categoryId);
    formData.append('expenseuser', this.expense.userId);
    formData.append('expensedelete', this.expense.deleteID);
    formData.append('expensereceipt', this.expense.receiptPath);

    formData.append('file', this.selectedFile ?? null, this.selectedFile?.name);

    //const headers = new HttpHeaders().set('Content-Type', 'multipart/form-data');

    this.Service.insertExpense(formData)
      .subscribe(
        event => {
          if (event) {
            this.expenseAdded.emit();
            //this.toastr.success('Expense Added successfully!');
            this.dialogRef.close(event);
            // window.alert("added");
            // window.location.reload();
          }
        },
        error => {
          console.error('Error adding expense:', error);
          this.toastr.error('Failed to add expense!'); // Handle error gracefully
        }
      );
  }
}

