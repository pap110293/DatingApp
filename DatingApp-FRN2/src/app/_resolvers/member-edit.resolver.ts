import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemeberEditResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService,
    private authService: AuthService
  ) {}

  resolve(): User | Observable<User> | Promise<User> {
    return this.userService.getUser(this.authService.decodeToken.nameid).pipe(
      catchError(error => {
        if (error.status === 404) {
          this.alertify.error('User not found');
        } else {
          this.alertify.error('problem retrieving data');
        }
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }
}
