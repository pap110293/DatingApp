import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { NgForm } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('formEdit') editForm: NgForm;
  user: User;

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private alertify: AlertifyService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadUser();
  }

  loadUser() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
  }

  updateUser() {
    const userId = this.authService.decodeToken.nameid;
    this.userService
      .updateUser(userId, this.user)
      .subscribe(
        (data: User) => {
          this.editForm.reset();
          this.alertify.success('Your profile updated successfully');
          this.router.navigate(['/members/' + userId]);
        },
        error => {
          this.alertify.error(error);
        }
      );
  }
}
