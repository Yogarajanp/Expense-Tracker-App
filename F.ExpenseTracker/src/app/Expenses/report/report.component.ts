import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ExpenseService } from '../../expense.service';
import { FooterComponent } from '../../footer/footer.component';
import { NavbarComponent } from '../../navbar/navbar.component';
import { ViewExpenseComponent } from '../view-expense/view-expense.component';

@Component({
  selector: 'app-report',
  standalone: true,
  imports: [NavbarComponent, FooterComponent, ViewExpenseComponent, CommonModule, FormsModule],
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent {
  expenses: any = [];
  sixMonthExpenses: any = [];
  reportData: any = [];
  @Output() dataEmitter = new EventEmitter<any>();

  year: any;
  month: any;
  fromDate: any;
  toDate: any;
  noDataMessage: any;

  private currentDate = new Date();
  private defaultYear = this.currentDate.getFullYear();
  private defaultMonth = this.currentDate.getMonth() + 1; // Months are zero-based
  private defaultFromDate = this.formatDate(this.currentDate);
  private defaultToDate = this.formatDate(this.currentDate);
  years: number[] = [];
  months = [
    { value: 1, name: 'January' },
    { value: 2, name: 'February' },
    { value: 3, name: 'March' },
    { value: 4, name: 'April' },
    { value: 5, name: 'May' },
    { value: 6, name: 'June' },
    { value: 7, name: 'July' },
    { value: 8, name: 'August' },
    { value: 9, name: 'September' },
    { value: 10, name: 'October' },
    { value: 11, name: 'November' },
    { value: 12, name: 'December' }
  ];

  formatDate(date: Date): string {
    // Format date as 'YYYY-MM-DD' or any other format required by your API
    const year = date.getFullYear();
    const month = ('0' + (date.getMonth() + 1)).slice(-2);
    const day = ('0' + date.getDate()).slice(-2);
    return `${year}-${month}-${day}`;
  }

  isModalOpen: boolean = false;
  modalTitle: string = '';
  reportType: string = '';

  constructor(private expenseService: ExpenseService) { }

  ngOnInit() {
    this.populateYears();
  }

  populateYears() {
    const currentYear = new Date().getFullYear();
    for (let i = currentYear; i >= 2000; i--) { // Change 2000 to your desired start year
      this.years.push(i);
    }
  }

  AssignFields() {
    this.year = this.year !== undefined && this.year !== null ? this.year : this.defaultYear;
    this.month = this.month !== undefined && this.month !== null ? this.month : this.defaultMonth;
    this.toDate = this.toDate !== undefined && this.toDate !== null ? this.toDate : this.defaultToDate;
    this.fromDate = this.fromDate !== undefined && this.fromDate !== null ? this.fromDate : this.defaultFromDate;
  }

  openModal(type: string) {
    this.isModalOpen = true;
    this.reportType = type;
    switch (type) {
      case 'monthly':
        this.modalTitle = 'Generate Monthly Report';
        break;
      case 'yearly':
        this.modalTitle = 'Generate Yearly Report';
        break;
      case 'specificDate':
        this.modalTitle = 'Generate Specific Date Report';
        break;
    }
  }

  closeModal() {
    this.isModalOpen = false;
  }

  generateSixMonthReport(reportType: string) {
    this.AssignFields();
    this.expenseService.getExpenseReport(this.year, this.month, this.fromDate, this.toDate, reportType).subscribe(
      data => {
        if (data.length === 0) {
          this.noDataMessage = 'No expenses found for the specified criteria.';
        } else {
          this.noDataMessage = null;
          console.log(data);
        }
        this.dataEmitter.emit(data);
      },
      error => {
        console.error('Error fetching report data:', error);
      }
    );
  }

  submitReport() {
    switch (this.reportType) {
      case 'monthly':
        this.generateReport('byMonth');
        break;
      case 'yearly':
        this.generateReport('byYear');
        break;
      case 'specificDate':
        this.generateReport('byDate');
        break;
    }
  }
  generateReport(reportType: string) {
    this.AssignFields();
    this.expenseService.getExpenseReport(this.year, this.month, this.fromDate, this.toDate, reportType).subscribe(
      data => {
        if (data.length === 0) {
          this.noDataMessage = 'No expenses found for the specified criteria.';
        } else {
          this.noDataMessage = null;
          console.log(data);
        }
        this.dataEmitter.emit(data);
      },
      error => {
        console.error('Error fetching report data:', error);
      }
    );
    this.closeModal();
  }
}