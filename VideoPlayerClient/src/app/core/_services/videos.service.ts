import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { UploadVideo, Video } from '../_models/video';

@Injectable({
  providedIn: 'root',
})
export class VideosService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getVideos() {
    return this.http.get<Video[]>(this.baseUrl + 'videos');
  }

  getVideo(id: string) {
    return this.http.get<Video>(this.baseUrl + 'videos/' + id);
  }

  getUserVideos(userId: string) {
    return this.http.get<Video[]>(this.baseUrl + 'videos/user/' + userId);
  }

  uploadVideo(model: UploadVideo) {
    return this.http.post(this.baseUrl + 'videos/upload', model);
  }
}
