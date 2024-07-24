import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { AuthService, User } from '@auth0/auth0-angular';
import { ApiGatewayUserManagementService } from './services/api-gateway-user-management.service';
import { BaseResponse } from './models/base.response';
import { AppUser } from './models/user';
import { NgToastModule, NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterModule, NgToastModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  toast = inject(NgToastService);

  constructor(private router : Router, private apiGatewayUserManagementService : ApiGatewayUserManagementService) {
    inject(AuthService).user$.subscribe((user) => {
      if (user) {
        this.createProfileIfNotExists(user);      
      }
      else {
        this.router.navigate(['/explore']);
      }
    });
  }

  private createProfileIfNotExists(user : User) {
    this.apiGatewayUserManagementService.createUserIfNotExists(user).subscribe({
      next : (response : BaseResponse<AppUser>) => {
        this.apiGatewayUserManagementService.setUser(response.value);
        this.ManagePostAuthenticationRouting(response);
      },
      error : (error : BaseResponse<AppUser>) => {
        this.toast.danger('An error occurred while creating your profile');
      }
    });
  }

  private ManagePostAuthenticationRouting(response: BaseResponse<AppUser>) {
    if (!response.value.IsProfileCompleted) {
      this.router.navigate(['/home/setup']);
    } else {
      this.router.navigate(['/home/dashboard']);
    }
  }
}