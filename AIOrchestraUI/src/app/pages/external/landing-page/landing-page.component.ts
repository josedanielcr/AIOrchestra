import { DOCUMENT } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'landing-page',
  standalone: true,
  imports: [
    RouterLink
  ],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent {
  public currYear: number = new Date().getFullYear();

  constructor(@Inject(DOCUMENT) public document: Document, public auth: AuthService) {}

  public goToGithub(): void {
    window.open('https://github.com/josedanielcr/AIOrchestra', '_blank');
  }
}
