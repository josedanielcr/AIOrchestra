import { Component } from '@angular/core';
import { ApiGatewayUserManagementService } from '../../../services/api-gateway-user-management.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {


  constructor(private userManagementService : ApiGatewayUserManagementService) { }

  ngOnInit(): void {
    console.log(this.userManagementService.user());    
  }
}
