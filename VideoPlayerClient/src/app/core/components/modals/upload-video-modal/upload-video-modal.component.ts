import { Component, OnInit } from '@angular/core';
import { UploadVideo, VideoData } from '../../../_models/video';
import { AccountService } from '../../../_services/account.service';
import { VideosService } from '../../../_services/videos.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-upload-video-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './upload-video-modal.component.html',
  styleUrl: './upload-video-modal.component.scss',
})
export class UploadVideoModalComponent implements OnInit {
  form: UploadVideo = {
    title: '',
    description: '',
    videoData: {
      videoType: '',
      bytes: '',
    },
    requestedById: '',
  };

  constructor(
    private accountService: AccountService,
    private videosService: VideosService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  uploadVideo() {
    const model = {
      title: this.form.title,
      description: this.form.description,
      videoData: {
        videoType: this.form.videoData.videoType,
        bytes: this.form.videoData.bytes,
      },
      requestedById: this.form.requestedById,
    };

    this.videosService.uploadVideo(model).subscribe({
      next: () => {
        this.router.navigateByUrl('/channel');
        this.toastr.success('Video uploaded successfully');
      },
      error: (error) => this.toastr.error(error[0].message),
    });
  }
}
