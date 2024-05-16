import { HttpClient } from '@angular/common/http';
import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { UserToken } from '../_models/userToken';
import { environment } from '../../../environments/environment';
import { LoginUser, RegisterUser } from '../_models/user';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private readonly platformId = inject(PLATFORM_ID);
  private currentUserSource = new BehaviorSubject<UserToken | null>(null);
  currentUser$ = this.currentUserSource.asObservable();
  get currentUserValue() {
    return this.currentUserSource.value;
  }

  constructor(private http: HttpClient) {}

  login(model: LoginUser) {
    return this.http
      .post<UserToken>(this.baseUrl + 'account/login', model)
      .pipe(
        map((response: UserToken) => {
          const user = response;
          if (user) {
            this.setCurrentUser(user);
            if (isPlatformBrowser(this.platformId)) {
              localStorage.setItem('user', JSON.stringify(user));
            }
          }
        })
      );
  }

  register(model: RegisterUser) {
    return this.http
      .post<UserToken>(this.baseUrl + 'account/register', model)
      .pipe(
        map((user) => {
          if (user) {
            if (isPlatformBrowser(this.platformId)) {
              localStorage.setItem('user', JSON.stringify(user));
            }
            this.currentUserSource.next(user);
          }
          return user;
        })
      );
  }

  setCurrentUser(user: UserToken) {
    this.currentUserSource.next(user);
  }

  logout() {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('user');
    }
    this.currentUserSource.next(null);
  }

  getUserFromLocalStorage() {
    if (isPlatformBrowser(this.platformId)) {
      const user = localStorage.getItem('user');
      return user ? JSON.parse(user) : null;
    }
    return null;
  }
}
