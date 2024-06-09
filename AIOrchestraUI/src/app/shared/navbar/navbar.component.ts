import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
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

  public currSegment : string = '';
  public user: AppUser | null = null;
  public isUserNavOpen: boolean = false;

  constructor(private route : ActivatedRoute,
    public userManagementService : ApiGatewayUserManagementService,
    @Inject(DOCUMENT) public document: Document, 
    public auth: AuthService
  ) { }

  ngOnInit() {
    const urlSegments = this.route.snapshot.url;
    this.currSegment = urlSegments[urlSegments.length - 1].path;
  }

  public toggleUserNav() {
    this.isUserNavOpen = !this.isUserNavOpen;
  }
}