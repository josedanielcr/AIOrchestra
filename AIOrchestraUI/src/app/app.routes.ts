import { Routes } from '@angular/router';
import { LandingPageComponent } from './pages/external/landing-page/landing-page.component';
import { InternalComponent } from './pages/internal/internal.component';
import { SetupComponent } from './pages/internal/setup/setup.component';
import { DashboardComponent } from './pages/internal/dashboard/dashboard.component';
import { PlaylistsComponent } from './pages/internal/playlists/playlists.component';

export const routes: Routes = [
    { path: '', redirectTo: '/explore', pathMatch: 'full' },
    { path: 'explore', component: LandingPageComponent },
    { path: 'home', component: InternalComponent, children: [
        { path: 'setup', component: SetupComponent },
        { path: 'dashboard', component: DashboardComponent },
        { path: 'playlists', component: PlaylistsComponent }
    ]}
];