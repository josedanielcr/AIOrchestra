import { Component, Input } from '@angular/core';
import { Playlist } from '../../models/playlist';
import { CommonModule } from '@angular/common';
import { MusicCardComponent } from '../music-card/music-card.component';
import { ApiGatewayUserManagementService } from '../../services/api-gateway-user-management.service';

@Component({
  selector: 'app-playlist-viewer',
  standalone: true,
  imports: [CommonModule, MusicCardComponent],
  templateUrl: './playlist-viewer.component.html',
  styleUrl: './playlist-viewer.component.css'
})
export class PlaylistViewerComponent {

  @Input() playlist: Playlist | null = null;
  public isOpen: boolean = false;

  constructor(public userManagementService : ApiGatewayUserManagementService) {}

  public open() {
    this.isOpen = true;
  }

  closeModal() {
    this.isOpen = false;
  }
}
