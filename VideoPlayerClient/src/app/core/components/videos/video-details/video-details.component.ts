import { Component, OnInit } from '@angular/core';
import { Video } from '../../../_models/video';
import { VideosService } from '../../../_services/videos.service';
import { ActivatedRoute } from '@angular/router';
import { NgClass, NgIf } from '@angular/common';

@Component({
  selector: 'app-video-details',
  standalone: true,
  imports: [NgIf, NgClass],
  templateUrl: './video-details.component.html',
  styleUrl: './video-details.component.scss',
})
export class VideoDetailsComponent implements OnInit {
  video: Video | undefined;
  likeButtonClass: string = 'material-icons-outlined';
  dislikeButtonClass: string = 'material-icons-outlined';

  constructor(
    private videosService: VideosService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadVideo();
    console.log(this.loadVideo());
  }

  loadVideo() {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;
    this.videosService.getVideo(id).subscribe({
      next: (video) => (this.video = video),
    });
  }

  toggleLike() {
    this.likeButtonClass =
      this.likeButtonClass === 'material-icons-outlined'
        ? 'material-icons'
        : 'material-icons-outlined';
    this.dislikeButtonClass = 'material-icons-outlined';
  }

  toggleDislike() {
    this.dislikeButtonClass =
      this.dislikeButtonClass === 'material-icons-outlined'
        ? 'material-icons'
        : 'material-icons-outlined';
    this.likeButtonClass = 'material-icons-outlined';
  }
}
