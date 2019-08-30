import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodeToken: any;

  constructor(private http: HttpClient) {}
  login(model: any) {
    const requestUrl = this.baseUrl + 'login';

    return this.http.post(requestUrl, model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          const rawToken = user.token;
          localStorage.setItem(environment.tokenLocalStoreKey, rawToken);
          this.decodeToken = this.jwtHelper.decodeToken(rawToken);
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
  }

  isLoggedIn() {
    const token = localStorage.getItem(environment.tokenLocalStoreKey);
    return !this.jwtHelper.isTokenExpired(token);
  }
}
