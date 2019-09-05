import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AlertifyService } from './alertify.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodeToken: any;
  currentUser: User;

  constructor(private http: HttpClient) {}
  login(model: any) {
    const requestUrl = this.baseUrl + 'login';

    return this.http.post(requestUrl, model).pipe(
      map((response: any) => {
        const token = response.token;
        if (token) {
          const rawToken = response.token;
          localStorage.setItem(environment.tokenLocalStoreKey, rawToken);
          this.decodeToken = this.jwtHelper.decodeToken(rawToken);
        }

        const user = response.user;
        if (user) {
          localStorage.setItem(
            environment.currentUserLocalStoreKey,
            JSON.stringify(user)
          );
          this.currentUser = user;
        }
      })
    );
  }

  register(model: any) {
    const requestUrl = this.baseUrl + 'register';
    return this.http.post(requestUrl, model);
  }

  logout() {
    localStorage.removeItem(environment.tokenLocalStoreKey);
    localStorage.removeItem(environment.currentUserLocalStoreKey);
    this.decodeToken = null;
    this.currentUser = null;
  }

  isLoggedIn() {
    const token = localStorage.getItem(environment.tokenLocalStoreKey);
    return !this.jwtHelper.isTokenExpired(token);
  }
}
