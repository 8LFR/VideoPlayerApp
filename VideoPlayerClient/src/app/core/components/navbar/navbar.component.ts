import { Component, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar'
import { MatIconModule } from '@angular/material/icon'
import { MatSidenavModule } from '@angular/material/sidenav'
import { SidebarComponent } from '../sidebar/sidebar.component';
import { HomeComponent } from './pages/home/home.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [ 
    CommonModule, 
    MatToolbarModule,
    MatIconModule,
    MatSidenavModule,
    SidebarComponent,
    HomeComponent,
    RouterOutlet
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent{
  collapsed = signal(false);

  sidenavWidth = computed(() => this.collapsed() ? '65px' : '250px');
}
