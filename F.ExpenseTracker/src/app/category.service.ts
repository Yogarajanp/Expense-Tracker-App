import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from './Environment/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }
  private readonly UserUrl = `${environment.apiUrl}/Category`;

  getAllCategories() {
    return this.http.get<any>(this.UserUrl);
  }

  insertCategory(category: any): Observable<any> {
    return this.http.post<any>(this.UserUrl, category);
  }
}

