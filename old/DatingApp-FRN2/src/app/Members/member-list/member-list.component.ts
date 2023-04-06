import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination } from 'src/app/_models/pagination';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(
    localStorage.getItem(environment.currentUserLocalStoreKey)
  );
  genderList = [
    { value: '', display: 'Gender' },
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' }
  ];
  userParams: any = {};
  pagination: Pagination;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.users = data.users.result;
      this.pagination = data.users.pagination;
    });

    this.resetParams();
  }

  resetFilters() {
    this.resetParams();
    this.loadUser();
  }

  private resetParams() {
    this.userParams.gender = '';
    this.userParams.maxAge = 99;
    this.userParams.minAge = 18;
    this.userParams.orderBy = 'lastActive';
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUser();
  }

  loadUser() {
    this.userService
      .getUsers(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.userParams
      )
      .subscribe(data => {
        this.users = data.result;
        this.pagination = data.pagination;
      });
  }
}