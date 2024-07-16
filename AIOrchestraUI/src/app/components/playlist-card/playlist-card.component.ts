import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { Playlist } from '../../models/playlist';
import { CommonModule } from '@angular/common';
import { ApiGatewayUserManagementService } from '../../services/api-gateway-user-management.service';
import { PlaylistViewerComponent } from '../playlist-viewer/playlist-viewer.component';

@Component({
  selector: 'app-playlist-card',
  standalone: true,
  imports: [CommonModule, PlaylistViewerComponent],
  templateUrl: './playlist-card.component.html',
  styleUrl: './playlist-card.component.css'
})
export class PlaylistCardComponent{

  @Input() playlist: Playlist | null = null;
  @ViewChild(PlaylistViewerComponent) playlistViewer: PlaylistViewerComponent | null = null;

  constructor(public userManagementService : ApiGatewayUserManagementService) {}

  public getPlaylistDuration() {
    if (this.playlist) {
      const durationInMs : number = this.playlist.Songs.reduce((acc, track) => acc + track.duration_ms, 0);
      const minutes = Math.floor(durationInMs / 60000);
      const seconds = ((durationInMs % 60000) / 1000).toFixed(0);
      return `${minutes}:${+seconds < 10 ? '0' : ''}${seconds}`;
    }
    return '0:00';
  }

  public viewPlaylist() {
    this.playlistViewer?.open();
  }
}