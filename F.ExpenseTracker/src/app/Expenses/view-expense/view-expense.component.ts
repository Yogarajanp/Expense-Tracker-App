import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ExpenseService } from '../../expense.service';
import { NavbarComponent } from '../../navbar/navbar.component';
import { AddExpenseComponent } from '../add-expense/add-expense.component';
import { ReportComponent } from '../report/report.component';

@Component({
  selector: 'app-view-expense',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule, NavbarComponent, MatPaginator, ReportComponent],
  templateUrl: './view-expense.component.html',
  styleUrl: './view-expense.component.css'
})
export class ViewExpenseComponent implements OnInit, AfterViewInit {


  expenseData!: any[];

  displayedColumns: string[] = ['S.No', 'Category', 'Amount', 'DateofTransaction', 'ModeofTransaction', 'Description', 'Receipt', 'Action'];
  dataSource = new MatTableDataSource<any>();


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  constructor(private expenseService: ExpenseService, public dialog: MatDialog) { }



  ngOnInit(): void {
    this.loadRecords();


  }


  ngAfterViewInit(): void {
    console.log(this.paginator);
    this.paginator.pageSize = 5;
    this.dataSource.paginator = this.paginator;
  }
  loadRecords() {

    this.expenseService.getAllExpense()
      .subscribe(
        data => {

          this.dataSource.data = data;

        },
        error => {

          console.error('data fetch error:', error);

        }
      );
  }

  updateData(data: any[]) {
    this.expenseData = data;
    this.dataSource.data = this.expenseData;
  }

  onReportDataReceived(data: any) {
    this.updateData(data);
  }

  async newExpense() {
    const dialogRef = this.dialog.open(AddExpenseComponent, {
      width: '1000px'
    });
    dialogRef.componentInstance.expenseAdded.subscribe(() => {
      this.loadRecords();
    })
  }

  async removeExpense(expenseId: any) {
    const response = await this.expenseService.deleteExpense(expenseId);
    if (response) {
      alert("Record deleted successfully");
      this.loadRecords();
      // window.location.reload();
    }


  }
  async editExpense(expense: any) {

  }
  download(expenseId: any, receiptPath: any) {

    this.expenseService.downloadReceipt(expenseId)
      .subscribe(blob => {
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;


        const filePath = receiptPath;
        const filename = this.getFilenameFromPath(filePath);
        console.log(filename);

        link.download = filename
        link.click();
        window.URL.revokeObjectURL(url);
      },
        error => {
          console.error('Error downloading receipt:', error);
        });
  }
  getFilenameFromPath(filePath: string): string {
    const url = new URL(filePath, 'file:');
    return url.pathname.split('\\').pop() || '';
  }

}

