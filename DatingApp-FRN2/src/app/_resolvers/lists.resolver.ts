import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { PaginatedResutl } from '../_models/pagination';

@Injectable()
export class ListsResolver implements Resolve<PaginatedResutl<User[]>> {
  pageNumber = 1;
  pageSize = 6;
  likesParam = 'Likers';

  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(): PaginatedResutl<User[]> | Observable<PaginatedResutl<User[]>> | Promise<PaginatedResutl<User[]>> {
    return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likesParam).pipe(
      catchError(error => {
        this.alertify.error('problem retrieving data');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
