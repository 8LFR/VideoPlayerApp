import { Routes } from '@angular/router';
import { HomeComponent } from './core/components/navbar/pages/home/home.component';
import { ChannelComponent } from './core/components/sidebar/pages/channel/channel.component';
import { LikedVideosComponent } from './core/components/sidebar/pages/liked-videos/liked-videos.component';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home',
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'channel',
    component: ChannelComponent,
  },
  {
    path: 'liked-videos',
    component: LikedVideosComponent,
  },
];
