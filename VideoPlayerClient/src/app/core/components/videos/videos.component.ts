import { Component } from '@angular/core';
import { Video } from '../../_models/video';
import { VideosService } from '../../_services/videos.service';

@Component({
  selector: 'app-videos',
  standalone: true,
  imports: [],
  templateUrl: './videos.component.html',
  styleUrl: './videos.component.scss',
})
export class VideosComponent {
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
