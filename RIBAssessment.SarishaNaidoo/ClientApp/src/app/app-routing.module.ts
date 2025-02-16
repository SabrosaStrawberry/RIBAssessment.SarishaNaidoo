import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserLoginComponent } from './pages/user-login/user-login.component';
import { EmployeeOverviewComponent } from './pages/employee-overview/employee-overview.component';
import { AuthGuard } from './shared/guards/auth.guard';

const routes: Routes = [
  { path: 'login', component: UserLoginComponent },
  { path: 'employees', component: EmployeeOverviewComponent, canActivate:[AuthGuard] }, 
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Redirect to login by default
  { path: '**', redirectTo: '/login' } // Redirect unknown routes to login
];

@NgModule({

  imports: [
    RouterModule.forRoot(
      routes
    )
  ],
  exports: [
    RouterModule
  ]
})

export class AppRoutingModule { }
