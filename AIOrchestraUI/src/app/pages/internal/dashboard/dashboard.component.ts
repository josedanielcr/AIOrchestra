import { Component } from '@angular/core';
import { ApiGatewayUserManagementService } from '../../../services/api-gateway-user-management.service';
import { ButtonComponent } from '../../../components/button/button.component';
import { ButtonType } from '../../../components/button/type.enum';
import { MusicRecommenderService } from '../../../services/music-recommender.service';
import { MusicPreferences } from '../../../models/music.preferences';
import { Song } from '../../../models/song';
import { CommonModule } from '@angular/common';
import { MusicCardComponent } from '../../../components/music-card/music-card.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ButtonComponent, CommonModule, MusicCardComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

  public ButtonType = ButtonType;
  public songs : Song[] = [];

  constructor(private userManagementService : ApiGatewayUserManagementService,
    private musicRecommenderService : MusicRecommenderService
  ) {
    this.recommend = this.recommend.bind(this);
    this.saveToPlaylist = this.saveToPlaylist.bind(this);
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
    console.log('saveToPlaylist');
  }
}
