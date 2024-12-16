import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from '../user.service';

import { ChangepasswordComponent } from '../changepassword/changepassword.component';
import { FooterComponent } from '../footer/footer.component';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, NavbarComponent, FooterComponent],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profile: any = { user: {} };
  error: string = '';
  success: string = '';
 userID:any;
  constructor(
    private userService: UserService,
    private dialog: MatDialog,
    @Inject(PLATFORM_ID) private platformId: Object
  ) { }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const currentUser = sessionStorage.getItem('currentUser');
      if (currentUser) {
        const userstorage = JSON.parse(currentUser);
        this.userID = userstorage.user.userID;
      }
      this.userService.getProfile(this.userID).subscribe(
        (data) => {
          this.profile = data;
          console.log(this.profile)
        },
        (error) => {
          this.error = 'Error fetching profile';

        }
      );
    }
  }

  changePassword(): void {
    const dialogRef = this.dialog.open(ChangepasswordComponent, {

  });


    dialogRef.afterClosed().subscribe(result => {
      if (result) {

      }
    });
  }
}



