import { Routes } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { NavbarComponent } from './navbar/navbar.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './Auth/auth.guard';
import { RegistrationComponent } from './registration/registration.component';
import { ProfileComponent } from './profile/profile.component';
import { ViewExpenseComponent } from './Expenses/view-expense/view-expense.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ReportComponent } from './Expenses/report/report.component';
import { AboutUsComponent } from './about-us/about-us.component';
import { ViewcategoryComponent } from './Admin/viewcategory/viewcategory.component';



export const routes: Routes = [
    {
        path: 'index',
        component: IndexComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'navbar',
        component: NavbarComponent
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'register',
        component: RegistrationComponent
    },
    {
        path: 'profile',
        component: ProfileComponent
    },
    {
        path: 'userexpense',
        component: ViewExpenseComponent,
        canActivate: [AuthGuard]

    },
    {
        path: 'Dashboard',
        component: DashboardComponent
    },
    {
        path: 'report',
        component: ReportComponent
    },
    {
        path: 'aboutus',
        component: AboutUsComponent
    },
    {
        path: 'viewCategory',
        component: ViewcategoryComponent
    }
    ,
    {
        path: '',
        redirectTo: 'register',
        pathMatch: 'prefix'
    },

];
