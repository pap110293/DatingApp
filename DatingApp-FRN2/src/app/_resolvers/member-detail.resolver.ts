import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable()
export class MemeberDetailResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(
    route: ActivatedRouteSnapshot
  ): User | Observable<User> | Promise<User> {
    return this.userService.getUser(route.params['id']).pipe(
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
