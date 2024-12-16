
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { IndexComponent } from './index/index.component';
import { LoginComponent } from './login/login.component';
import { NavbarComponent } from './navbar/navbar.component';
import { ProfileComponent } from './profile/profile.component';
import { RegistrationDialogComponent } from './registration-dialog/registration-dialog.component';
import { RegistrationComponent } from './registration/registration.component';

import { AboutUsComponent } from './about-us/about-us.component';
import { DashboardComponent } from './dashboard/dashboard.component';



@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, IndexComponent, NavbarComponent, LoginComponent, RegistrationComponent,
    RegistrationDialogComponent, ProfileComponent, DashboardComponent, AboutUsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'ExpenseTraker';
}
