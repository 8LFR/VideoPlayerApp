import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-video-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './video-list.component.html',
  styleUrl: './video-list.component.scss',
})
export class VideoListComponent implements OnInit {
  videos: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any[]>('https://localhost:7089/api/videos').subscribe({
      next: (response) => {
        this.videos = response;

        this.videos.forEach((video) => {
          this.getVideoThumbnail(video.filePathOrUrl).then((thumbnail) => {
            video.thumbnail = thumbnail;
          });
        });
      },
      error: (error) => console.log(error),
      complete: () => console.log('Request has completed'),
    });
  }

  async getVideoThumbnail(filePath: string): Promise<string> {
    return filePath;
  }
}
