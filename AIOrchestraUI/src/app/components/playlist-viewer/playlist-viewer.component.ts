import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Playlist } from '../../models/playlist';
import { CommonModule } from '@angular/common';
import { MusicCardComponent } from '../music-card/music-card.component';
import { ApiGatewayUserManagementService } from '../../services/api-gateway-user-management.service';
import { PlaylistService } from '../../services/playlist.service';

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
  public isModalDetailOpen: boolean = false;
  @Output() playlistDeleted = new EventEmitter<string>();

  constructor(public userManagementService : ApiGatewayUserManagementService,
    private playlistService : PlaylistService
  ) {}

  public open() {
    this.isOpen = true;
  }

  closeModal() {
    this.isOpen = false;
  }

  toggleModalDetail() {
    this.isModalDetailOpen = !this.isModalDetailOpen;
  }

  deletePlaylist() {
    this.playlistService.deletePlaylist(this.playlist?.Id as string).subscribe({
      next: () => {
        this.isOpen = false;
        this.playlistDeleted.emit(this.playlist?.Id as string);
      },
      error: (error) => {
      }
    });
  }
}
