import { Routes } from '@angular/router';
import { HomeComponent } from './core/components/home/home.component';
import { VideosComponent } from './core/components/videos/videos.component';
import { ChannelComponent } from './core/components/channel/channel.component';
import { LikedVideosComponent } from './core/components/liked-videos/liked-videos.component';
import { authGuard } from './core/_guards/auth.guard';
import { TestErrorComponent } from './core/components/errors/test-error/test-error.component';
import { NotFoundComponent } from './core/components/errors/not-found/not-found.component';
import { ServerErrorComponent } from './core/components/errors/server-error/server-error.component';
import { VideoDetailsComponent } from './core/components/videos/video-details/video-details.component';
import { ChannelDetailsComponent } from './core/components/channel/channel-details/channel-details.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: '',
    children: [
      {
        path: 'videos',
        component: VideosComponent,
      },
      {
        path: 'videos/:id',
        component: VideoDetailsComponent,
      },
      {
        path: 'channel',
        component: ChannelComponent,
      },
      {
        path: 'channel/:id',
        component: ChannelDetailsComponent,
      },
      {
        path: 'liked-videos',
        component: LikedVideosComponent,
      },
    ],
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
  },
  {
    path: 'errors',
    component: TestErrorComponent,
  },
  {
    path: 'not-found',
    component: NotFoundComponent,
  },
  {
    path: 'server-error',
    component: ServerErrorComponent,
  },
  {
    path: '**',
    component: NotFoundComponent,
    pathMatch: 'full',
  },
];
