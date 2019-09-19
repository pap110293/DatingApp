import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodeToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

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

        this.changeMemberPhoto(this.currentUser.photoUrl);
      })
    );
  }

  register(user: any) {
    const requestUrl = this.baseUrl + 'register';
    return this.http.post(requestUrl, user);
  }

  logout() {
    localStorage.removeItem(environment.tokenLocalStoreKey);
    localStorage.removeItem(environment.currentUserLocalStoreKey);
    this.decodeToken = null;
    this.currentUser = null;
  }

  isLoggedIn() {
    const token = localStorage.getItem(environment.tokenLocalStoreKey);

    if (!token) {
      return false;
    }

    return !this.jwtHelper.isTokenExpired(token);
  }

  changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
    this.currentUser.photoUrl = photoUrl;
    localStorage.setItem(
      environment.currentUserLocalStoreKey,
      JSON.stringify(this.currentUser)
    );
  }

  getCurrentUserId() {
    return this.decodeToken.nameid;
  }
}
