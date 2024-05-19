import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UploadVideo } from '../../../_models/video';
import { AccountService } from '../../../_services/account.service';
import { VideosService } from '../../../_services/videos.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-upload-video-modal',
  standalone: true,
  imports: [FormsModule, MatFormFieldModule, MatProgressSpinnerModule, NgIf],
  templateUrl: './upload-video-modal.component.html',
  styleUrl: './upload-video-modal.component.scss',
})
export class UploadVideoModalComponent implements OnInit {
  @Output() videoUploaded = new EventEmitter<void>();

  form: UploadVideo = {
    title: '',
    description: '',
    videoData: {
      videoType: '',
      bytes: '',
    },
    requestedById: '',
  };
  heroForm: any;

  constructor(
    private accountService: AccountService,
    private videosService: VideosService,
    private router: Router,
    private toastr: ToastrService,
    public dialogRef: MatDialogRef<UploadVideoModalComponent>
  ) {}

  ngOnInit(): void {}

  uploadVideo() {
    if (this.accountService.currentUserValue) {
      const model = {
        title: this.form.title,
        description: this.form.description,
        videoData: {
          videoType: this.form.videoData.videoType,
          bytes: this.form.videoData.bytes,
        },
        requestedById: this.accountService.currentUserValue.id,
      };

      this.videosService.uploadVideo(model).subscribe({
        next: () => {
          this.videoUploaded.emit();
          this.router.navigateByUrl('/channel');
          this.toastr.success('Video uploaded successfully');
          this.cancel();
        },
        error: (error) => this.toastr.error(error[0].message),
      });
    }
  }

  cancel() {
    this.dialogRef.close(true);
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    console.log('input', input.files);
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        const result = e.target?.result as string;
        this.form.videoData.bytes = result.split(',')[1];
        this.form.videoData.videoType = file.type;
      };
      reader.readAsDataURL(file);
    }
  }
}
