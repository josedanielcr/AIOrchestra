import { Routes } from '@angular/router';
import { LandingPageComponent } from './pages/external/landing-page/landing-page.component';

export const routes: Routes = [
    { path: '', redirectTo: '/explore', pathMatch: 'full' },
    { path: 'explore', component: LandingPageComponent }
];