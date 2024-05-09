import { Routes } from '@angular/router';
import { HomeComponent } from './core/components/home/home.component';
import { VideosComponent } from './core/components/videos/videos.component';
import { ChannelComponent } from './core/components/channel/channel.component';
import { LikedVideosComponent } from './core/components/liked-videos/liked-videos.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'videos',
    component: VideosComponent,
  },
  {
    path: 'channel',
    component: ChannelComponent,
  },
  {
    path: 'liked-videos',
    component: LikedVideosComponent,
  },
  {
    path: '**',
    component: HomeComponent,
    pathMatch: 'full',
  },
];
