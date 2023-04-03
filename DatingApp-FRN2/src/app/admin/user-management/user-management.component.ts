import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { RoleModalComponent } from '../role-modal/role-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: User[];
  bsModalRef: BsModalRef;

  constructor(
    private adminService: AdminService,
    private alertify: AlertifyService,
    private modalService: BsModalService
  ) {}

  ngOnInit() {
    this.getUserWithRoles();
  }

  getUserWithRoles() {
    this.adminService.getUserWithRole().subscribe(
      (users: User[]) => {
        this.users = users;
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  showEditRolesModal(user: User) {
    const initialState = {
      user,
      userRoles: user.roles
    };
    this.bsModalRef = this.modalService.show(RoleModalComponent, {
      initialState
    });
    this.bsModalRef.content.updateSelectedRoles.subscribe(
      data => {
        this.users.filter(i => i.id === data.user.id)[0].roles = data.newRoles;
        this.bsModalRef.hide();
      },
      error => this.alertify.error(error)
    );
  }
}
