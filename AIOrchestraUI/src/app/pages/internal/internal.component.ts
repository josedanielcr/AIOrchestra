import { Component } from '@angular/core';
import { NavbarComponent } from '../../shared/navbar/navbar.component';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../../shared/sidebar/sidebar.component';


@Component({
  selector: 'app-internal',
  standalone: true,
  imports: [NavbarComponent, RouterOutlet, SidebarComponent],
  templateUrl: './internal.component.html',
  styleUrl: './internal.component.css'
})
export class InternalComponent {
}
