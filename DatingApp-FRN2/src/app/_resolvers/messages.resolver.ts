import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { PaginatedResutl } from '../_models/pagination';
import { AuthService } from '../_services/auth.service';
import { Message } from '../_models/Message';

@Injectable()
export class MessagesResolver implements Resolve<PaginatedResutl<Message[]>> {
  pageNumber = 1;
  pageSize = 6;
  messageContainer = 'Unread';

  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService,
    private authService: AuthService
  ) {}

  resolve(): Observable<PaginatedResutl<Message[]>> {
    return this.userService
      .getMessages(this.authService.getCurrentUserId(),this.pageNumber, this.pageSize, this.messageContainer)
      .pipe(
        catchError(error => {
          this.alertify.error('problem retrieving message');
          this.router.navigate(['/home']);
          return of(null);
        })
      );
  }
}
