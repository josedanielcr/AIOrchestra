import { Component, inject, OnInit } from '@angular/core';
import { Playlist } from '../../../models/playlist';
import { PlaylistService } from '../../../services/playlist.service';
import { ApiGatewayUserManagementService } from '../../../services/api-gateway-user-management.service';
import { BaseResponse } from '../../../models/base.response';
import { CommonModule } from '@angular/common';
import { PlaylistCardComponent } from '../../../components/playlist-card/playlist-card.component';
import { MusicRecommenderService } from '../../../services/music-recommender.service';
import { Song } from '../../../models/song';
import { NgToastService } from 'ng-angular-popup';

@Component({
  selector: 'app-playlists',
  standalone: true,
  imports: [CommonModule, PlaylistCardComponent],
  templateUrl: './playlists.component.html',
  styleUrl: './playlists.component.css'
})
export class PlaylistsComponent implements OnInit {

  public playlists : Playlist[] = [];
  toast = inject(NgToastService);

  constructor(private playlistService : PlaylistService,
    private userService : ApiGatewayUserManagementService,
    private musicRecommenderService : MusicRecommenderService
  ){}

  ngOnInit(): void {
    this.getUserPlaylists();
  }

  private getUserPlaylists() {
    this.playlistService.getUserPlaylists(this.userService.user()?.Id as string).subscribe({
      next : (result : any) => {
        const baseResponse = result as BaseResponse<Playlist[]>;
        this.fillPlaylistSongs(baseResponse.value);
      },
      error : (error : undefined) => {
        this.toast.danger('An error occurred while fetching your playlists');
      } 
    });
  }

  private fillPlaylistSongs(value: Playlist[]) {
    let songsIds : string[] = [];
    value.forEach((playlist : Playlist) => {
      songsIds = playlist.SongIds;
      this.executeGetSongsRequest(songsIds, playlist);
    });
  }

  private executeGetSongsRequest(songsIds: string[], playlist : Playlist) {
    this.musicRecommenderService.getSongsById(songsIds).subscribe({
      next : (result : any) => {
        const baseResponse = result as BaseResponse<Song[]>;
        playlist.Songs = baseResponse.value;
        this.playlists.push(playlist);
      },
      error : (error : undefined) => {
        this.toast.danger('An error occurred while fetching your songs');
      }
    });
  }

  public deletePlaylist($event: string) {
    this.playlists = this.playlists.filter((playlist : Playlist) => playlist.Id !== $event);
  }
}