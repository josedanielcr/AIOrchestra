import { Routes } from '@angular/router';
import { LandingPageComponent } from './pages/external/landing-page/landing-page.component';
import { InternalComponent } from './pages/internal/internal.component';

export const routes: Routes = [
    { path: '', redirectTo: '/explore', pathMatch: 'full' },
    { path: 'explore', component: LandingPageComponent },
    { path: 'home', component: InternalComponent }
];