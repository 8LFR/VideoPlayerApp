import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { VideoListComponent } from './video-list/video-list.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, HttpClientModule, VideoListComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  title = 'Home';

  videos: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('https://localhost:7089/api/videos').subscribe({
      next: (response) => (this.videos = response),
      error: (error) => console.log(error),
      complete: () => console.log('Request has completed'),
    });
  }
}
