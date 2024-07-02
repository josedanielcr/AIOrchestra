import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreatePlaylistReq } from '../models/requests/create.playlist.req';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PlaylistService {

  constructor(private http : HttpClient) { }

  public createPlaylist(playlistReq : CreatePlaylistReq) {
    return this.http.post(environment.apiGateway+'/api/playlist', playlistReq);
  }

  public getUserPlaylists(userId : string) {
    return this.http.post(environment.apiGateway+'/api/playlist/user',{userId : userId});
  }
}