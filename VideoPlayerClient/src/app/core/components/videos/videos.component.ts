import { Component, OnInit } from '@angular/core';
import { Video } from '../../_models/video';
import { VideosService } from '../../_services/videos.service';
import { NgFor } from '@angular/common';
import { VideoTileComponent } from './video-tile/video-tile.component';

@Component({
  selector: 'app-videos',
  standalone: true,
  imports: [NgFor, VideoTileComponent],
  templateUrl: './videos.component.html',
  styleUrl: './videos.component.scss',
})
export class VideosComponent implements OnInit {
  videos: Video[] = [];

  constructor(private videosService: VideosService) {}

  ngOnInit(): void {
    this.loadVideos();
  }

  loadVideos() {
    this.videosService.getVideos().subscribe({
      next: (videos) => (this.videos = videos),
    });
  }
}
