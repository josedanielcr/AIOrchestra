import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ApiGatewayUserManagementService } from '../../services/api-gateway-user-management.service';
import { AsyncPipe, DOCUMENT } from '@angular/common';
import { AppUser } from '../../models/user';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, AsyncPipe],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {

  public currSegment : string | undefined = '';
  public user: AppUser | null = null;
  public isUserNavOpen: boolean = false;

  constructor(public userManagementService : ApiGatewayUserManagementService,
    @Inject(DOCUMENT) public document: Document, 
    public auth: AuthService
  ) { }

  ngOnInit() {
    const currentUrl = window.location.href;
    const segments = currentUrl.split('/');
    this.currSegment = segments.pop();
    this.currSegment = this.currSegment ? this.currSegment.charAt(0).toUpperCase() + this.currSegment.slice(1) : '';
  }

  public toggleUserNav() {
    this.isUserNavOpen = !this.isUserNavOpen;
  }
}