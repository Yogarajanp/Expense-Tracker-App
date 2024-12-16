import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from './Environment/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  user = {
    deleteID: 1,
    mail: 'gjhjhjk@gmail.com',
    password: '',
    userID: 1,
    userName: 'hbcbsh',

  };
  updatedUser = {
    Mail: '',
    Password: '',

    UserName: '',
    DeleteID: ''
  };
  constructor(private http: HttpClient) {


    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      const userstorage = JSON.parse(currentUser);
      // Assuming the username is stored under a 'username' property in the user object
      this.user.userName = userstorage.user.userName;
      this.user.userID = userstorage.user.userID;
      this.user.mail = userstorage.user.mail;
      this.user.password = userstorage.user.password;
      this.user.deleteID = userstorage.user.deleteID;

    }
  }
  private readonly UserUrl = `${environment.apiUrl}/User`;
  private readonly UserRoleUrl = `${environment.apiUrl}/UserRole/ByUser`;

  RegisterPost(formData: any) {

    var response = this.http.post<any>(this.UserUrl, formData)
    return response;
  }
  async GetUser(Mail: string,Password:string) {
    var response = await fetch(this.UserUrl + "/Login/" + Mail+"/"+Password);
    if (response.ok) {
      var userData = response.json();
      return userData;
    }
    else {
      console.log("No user exists");
    }
  }
  async Logout() {
    localStorage.removeItem('token');
    sessionStorage.removeItem('currentUser');
  }

  // checkUsernameAvailability(Mail: string) {
  //   return this.http.get<boolean>(this.UserUrl + "ByMail/" + Mail);
  // }



  getProfile(id: any): Observable<any> {
    return this.http.get<any>(this.UserUrl + "/ById/"+id);
  }


  changePassword(passwordData: any): Observable<any> {
    this.user.password = passwordData;
    console.log(this.user.userID);
    console.log(this.UserUrl);
    console.log(`${this.UserUrl}/${this.user.userID}`, this.user);

    var res = this.http.put<any>(`${this.UserUrl}/${this.user.userID}`, this.user);
    return res;
  }
  async isAdmin() {
    return this.http.get<any>(`${this.UserRoleUrl}/${this.user.userID}`);

  }

}


