import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ApiGatewayUserManagementService } from '../../services/api-gateway-user-management.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {

  public currSegment : string = '';

  constructor(private route : ActivatedRoute,
    public userManagementService : ApiGatewayUserManagementService
  ) { }

  ngOnInit() {
    const urlSegments = this.route.snapshot.url;
    this.currSegment = urlSegments[urlSegments.length - 1].path;
  }
}