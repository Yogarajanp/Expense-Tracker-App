import { Component, OnInit } from '@angular/core';
//import { UserService } from '../user.service';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../user.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent implements OnInit {
  isAdmin: boolean = false;
  constructor(private service: UserService, private router: Router) { }

  ngOnInit(): void {
    this.checkIsAdmin();
  }
  logout() {
    this.service.Logout();
    this.router.navigate(['/login']);

  }

  async checkIsAdmin() {
    (await this.service.isAdmin()).subscribe(
      data => {
        if (data.roleId == 1) {
          this.isAdmin = true;
        }

      },
      error => {

        console.error('data fetch error:', error);

      }
    );


  }

}


