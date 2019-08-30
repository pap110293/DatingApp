import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemeberListResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(): User | Observable<User> | Promise<User> {
    return this.userService.getUsers().pipe(
      catchError(error => {
        this.alertify.error('problem retrieving data');
        this.router.navigate(['/members']);
        return of(null);
      })
    );
  }
}
