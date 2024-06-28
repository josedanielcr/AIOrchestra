import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { Song } from '../../models/song';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-music-card',
  standalone: true,
  imports: [NgClass],
  templateUrl: './music-card.component.html',
  styleUrl: './music-card.component.css'
})
export class MusicCardComponent {

  @Input() song : Song = undefined as any;
  public isPlaying : boolean = false;
  public timeLeft: number = 0;
  @ViewChild('audioPlayer') audioPlayerRef!: ElementRef<HTMLAudioElement>;

  public togglePlayButton(){
    this.isPlaying = !this.isPlaying;
    if(this.isPlaying){
      this.playAudio(this.audioPlayerRef.nativeElement);
    } else {
      this.pauseAudio(this.audioPlayerRef.nativeElement);
    }
  }

  public pauseAudio(audioPlayer: HTMLAudioElement) {
    audioPlayer.pause();
  }

  public playAudio(audioPlayer: HTMLAudioElement): void {
    audioPlayer.play();
  }

  public pausePlayButton(){
    this.isPlaying = false;
    this.timeLeft = 0;
  }

  public updateTime(audioPlayer: HTMLAudioElement): void {
    const currentTime = audioPlayer.currentTime;
    const duration = audioPlayer.duration;
    this.timeLeft = Math.max(0, Math.floor(duration - currentTime));
  }
}
