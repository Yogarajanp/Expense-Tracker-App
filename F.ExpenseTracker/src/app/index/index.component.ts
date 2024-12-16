import { Component, OnInit } from '@angular/core';
import { FooterComponent } from '../footer/footer.component';
import { NavbarComponent } from '../navbar/navbar.component';


@Component({
  selector: 'app-index',
  standalone: true,
  imports: [NavbarComponent, FooterComponent],
  templateUrl: './index.component.html',
  styleUrl: './index.component.scss'
})
export class IndexComponent implements OnInit {
  userName: string = '';

  constructor() { }

  ngOnInit(): void {

    const currentUser = sessionStorage.getItem('currentUser');
    if (currentUser) {
      const userstorage = JSON.parse(currentUser);

      this.userName = userstorage.user.userName;
    }
  }
}


