import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
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
  canActivate(next: ActivatedRouteSnapshot): boolean {
    const roles = next.firstChild.data.roles as Array<string>;
    if (roles) {
      const match = this.authService.roleMatch(roles);
      if (match) {
        return true;
      }
      this.router.navigate(['members']);
      this.alerify.error('You are not authorised to access this area');
    }
    if (this.authService.isLoggedIn()) {
      return true;
    }

    this.alerify.error('You shall not pass!!!');
    this.router.navigate(['/']);
    return false;
  }
}
