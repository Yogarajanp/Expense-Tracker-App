import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { error } from 'console';
import { response } from 'express';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from './Environment/environment';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
  user = {
    DeleteID: 1,
    Mail: '',
    Password: '',
    UserID: 1,
    UserName: '',

  };

  private readonly UserUrl = `${environment.apiUrl}/Expense`;

  constructor(private http: HttpClient) {
    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      const userstorage = JSON.parse(currentUser);
      // Assuming the username is stored under a 'username' property in the user object
      this.user.UserName = userstorage.user.userName;
      this.user.UserID = userstorage.user.userID;
      this.user.Mail = userstorage.user.mail;
      this.user.Password = userstorage.user.password;
      this.user.DeleteID = userstorage.user.deleteID;

    }
  }


  getAllExpense() {
    return this.http.get<any>(this.UserUrl + "/ByUser/" + this.user.UserID);
  }

  getExpenseReport(year: any, month: any, fromDate: any, toDate: any, reportType: string) {

    return this.http.get<any>(`${this.UserUrl}/Report/${this.user.UserID}/${year}/${month}
      /${fromDate}/${toDate}/${reportType}`);

  }


  //expenseDetails.userId=this.user.UserID;
  //  expenseDetails.receiptPath='';


  insertExpense(formData: FormData): Observable<any> {
    return this.http.post<any>(this.UserUrl, formData);
  }



  async deleteExpense(deleteId: any) {
    const response = await this.http.delete<any>(this.UserUrl + "/" + deleteId).subscribe({
      next: (response) => {
        return response;
      },
      error: (Error) => {
        return Error;
      }
    });


    // const response=await  this.http.put<any>(this.UserUrl+"/"+deleteId).subscribe({
    //   next:(response)=>{
    //     return response;
    //   },
    //   error:(Error)=>{
    //     return Error;
    //   }
    // });
    return response;
  }


  editExpense(expense: any) {

  }
  downloadReceipt(expenseId: any) {

    return this.http.get(this.UserUrl + "/Download/" + expenseId, { responseType: 'blob' });
  }

  groupByExpense(year: number, month: number) {
    return this.http.get<any>(`${this.UserUrl}/groupBy/${this.user.UserID}/${year}/${month}`);

  }
}



