import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-role-modal',
  templateUrl: './role-modal.component.html',
  styleUrls: ['./role-modal.component.css']
})
export class RoleModalComponent implements OnInit {
  @Output() updateSelectedRoles = new EventEmitter();
  user: User;
  userRoles: string[];
  availableRoles = [];

  constructor(
    public bsModalRef: BsModalRef,
    private adminService: AdminService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.adminService.getAllRoles().subscribe(
      (roles: string[]) => {
        roles.forEach(role => {
          const availableRole = {
            name: role,
            value: role,
            checked: this.user.roles.includes(role)
          };
          this.availableRoles.push(availableRole);
        });
      },
      error => this.alertify.error(error)
    );
  }

  updateRoles() {
    const newRoles = this.availableRoles
      .filter(i => i.checked)
      .map(i => i.name);

    this.adminService.editRoles(this.user, newRoles).subscribe(
      roles => {
        this.updateSelectedRoles.emit({ user: this.user, newRoles: roles });
      },
      error => this.alertify.error(error)
    );
  }
}
