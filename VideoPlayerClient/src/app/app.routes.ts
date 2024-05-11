import { Routes } from '@angular/router';
import { HomeComponent } from './core/components/home/home.component';
import { VideosComponent } from './core/components/videos/videos.component';
import { ChannelComponent } from './core/components/channel/channel.component';
import { LikedVideosComponent } from './core/components/liked-videos/liked-videos.component';
import { authGuard } from './core/_guards/auth.guard';
import { TestErrorComponent } from './core/components/errors/test-error/test-error.component';
import { NotFoundComponent } from './core/components/errors/not-found/not-found.component';
import { ServerErrorComponent } from './core/components/errors/server-error/server-error.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
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
    ],
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
