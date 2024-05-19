import { NgFor, NgIf } from '@angular/common';
import { User } from '../../_models/user';
import { UsersService } from '../../_services/users.service';
import { AccountService } from '../../_services/account.service';
import { ChannelDetailsComponent } from './channel-details/channel-details.component';
import { Component, OnInit } from '@angular/core';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { VideosService } from '../../_services/videos.service';
import { Video } from '../../_models/video';
import { RouterLink } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';
import { UploadVideoModalComponent } from '../modals/upload-video-modal/upload-video-modal.component';

@Component({
  selector: 'app-channel',
  standalone: true,
  imports: [
    NgIf,
    NgFor,
    ChannelDetailsComponent,
    TabsModule,
    RouterLink,
    UploadVideoModalComponent,
    MatDialogModule,
  ],
  templateUrl: './channel.component.html',
  styleUrls: ['./channel.component.scss'],
})
export class ChannelComponent implements OnInit {
  user!: User;
  currentUserId!: string;
  videos!: Video[];

  constructor(
    private usersService: UsersService,
    public accountService: AccountService,
    private videosService: VideosService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.currentUserId = this.accountService.currentUserValue?.id || '';
    this.loadUser();
    this.getUserVideos();
  }

  loadUser() {
    this.usersService.getUser(this.currentUserId).subscribe((user) => {
      this.user = user;
    });
  }

  getUserVideos() {
    this.videosService.getUserVideos(this.currentUserId).subscribe((videos) => {
      this.videos = videos;
    });
  }

  isUserOwnChannel() {
    return this.accountService.currentUserValue?.id == this.user.id;
  }

  openUploadVideoModal() {
    const dialogRef = this.dialog.open(UploadVideoModalComponent, {
      width: '400px',
    });

    dialogRef.componentInstance.videoUploaded.subscribe(() => {
      this.onVideoUploaded();
    });
  }

  onVideoUploaded() {
    this.getUserVideos();
  }
}
