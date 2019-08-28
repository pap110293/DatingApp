import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private alerify: AlertifyService
  ) {}
  canActivate(): boolean {
    if (this.authService.isLoggedIn()) {
      return true;
    }

    this.alerify.error('You shall not pass!!!');
    this.router.navigate(['/home']);
    return false;
  }
}
