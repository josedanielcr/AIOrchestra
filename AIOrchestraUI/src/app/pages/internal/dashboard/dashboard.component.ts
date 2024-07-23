import { Component, ViewChild } from '@angular/core';
import { ApiGatewayUserManagementService } from '../../../services/api-gateway-user-management.service';
import { ButtonComponent } from '../../../components/button/button.component';
import { ButtonType } from '../../../components/button/type.enum';
import { MusicRecommenderService } from '../../../services/music-recommender.service';
import { MusicPreferences } from '../../../models/music.preferences';
import { Song } from '../../../models/song';
import { CommonModule } from '@angular/common';
import { MusicCardComponent } from '../../../components/music-card/music-card.component';
import { PlaylistService } from '../../../services/playlist.service';
import { CreatePlaylistReq } from '../../../models/requests/create.playlist.req';
import { InputComponent } from '../../../components/input/input.component';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { SetupComponent } from '../setup/setup.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ButtonComponent, CommonModule, MusicCardComponent, ReactiveFormsModule, InputComponent, SetupComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

  @ViewChild('recommendButton') recommendButton!: ButtonComponent;
  public ButtonType = ButtonType;
  public songs : Song[] = [];
  public newPlaylistForm! : FormGroup;
  public btnText : string = 'Click to save your new playlist!';
  public isSetupOpen : boolean = false;


  constructor(private userManagementService : ApiGatewayUserManagementService,
    private musicRecommenderService : MusicRecommenderService,
    private playlistService : PlaylistService,
    private fb : FormBuilder,
    private router : Router
  ) {
    this.recommend = this.recommend.bind(this);
    this.saveToPlaylist = this.saveToPlaylist.bind(this);
    this.CreateNewPlaylistForm();
   }

  private CreateNewPlaylistForm() {
    this.newPlaylistForm = this.fb.group({
      playlistName: ['']
    });
  }

  ngOnInit(): void {
    console.log(this.userManagementService.user());    
  }

  public recommend() {
    const userMusicPreferences : MusicPreferences = this.createUserMusicPreferences();
    this.musicRecommenderService.recommend(userMusicPreferences).subscribe((response) => {
      this.songs = response.value;
    });
  }

  private createUserMusicPreferences() {
    return new MusicPreferences(this.userManagementService.user()?.Danceability as number,
      this.userManagementService.user()?.Energy as number,
      this.userManagementService.user()?.Loudness as number,
      this.userManagementService.user()?.Speechiness as number,
      this.userManagementService.user()?.Instrumentalness as number,
      this.userManagementService.user()?.Liveness as number,
      []);
  }

  public saveToPlaylist() {
    const playlistReq = new CreatePlaylistReq(this.newPlaylistForm.value.playlistName as string,
      this.userManagementService.user()?.Id as string,
      this.songs.map(song => song.track_id));
    this.playlistService.createPlaylist(playlistReq).subscribe(() => {
      this.songs = [];
      this.router.navigate(['/home/playlists']);
    });
  }

  toggleSetup() {
    this.isSetupOpen = !this.isSetupOpen;
    if(this.isSetupOpen) this.recommendButton.disabled = true;
    else this.recommendButton.disabled = false;
  }

  closeSetupSection(){
    this.isSetupOpen = false;
  }
}