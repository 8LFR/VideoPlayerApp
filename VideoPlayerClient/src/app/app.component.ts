import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { AccountService } from './core/_services/account.service';
import { UserToken } from './core/_models/userToken';
import { HomeComponent } from './core/components/home/home.component';
import { SharedModule } from './core/_modules/shared.module';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    NavbarComponent,
    CommonModule,
    HomeComponent,
    SharedModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'VideoPlayer';
  users: any;

  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: UserToken = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
}
