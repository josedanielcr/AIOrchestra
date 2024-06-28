import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseResponse } from '../models/base.response';
import { environment } from '../../environments/environment.development';
import { MusicPreferences } from '../models/music.preferences';
import { Song } from '../models/song';

@Injectable({
  providedIn: 'root'
})
export class MusicRecommenderService {

  constructor(private http : HttpClient) { }

  public recommend(preferences : MusicPreferences): Observable<BaseResponse<Song[]>> {
    return this.http.post<BaseResponse<Song[]>>(environment.apiGateway+'/api/songs/recommend', preferences);
  }
}
