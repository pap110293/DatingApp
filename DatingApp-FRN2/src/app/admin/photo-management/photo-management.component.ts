import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { ActivatedRoute } from '@angular/router';
import { AdminService } from 'src/app/_services/admin.service';
import { Pagination } from 'src/app/_models/pagination';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  photos: Photo[];
  pagination: Pagination;

  constructor(
    private adminService: AdminService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.initData();
  }

  initData() {
    const currentPage = 1;
    const itemsPerPage = 6;
    this.adminService
      .getWatingForApprovePhotos(currentPage, itemsPerPage)
      .subscribe(data => {
        this.photos = data.result;
        this.pagination = data.pagination;
      });
  }

  loadData() {
    this.adminService
      .getWatingForApprovePhotos(
        this.pagination.currentPage,
        this.pagination.itemsPerPage
      )
      .subscribe(data => {
        this.photos = data.result;
        this.pagination = this.pagination;
      });
  }

  pageChanged(event) {
    this.pagination.currentPage = event.page;
    this.loadData();
  }

  approvePhoto(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe(
      () => {
        this.loadData();
      },
      error => this.alertify.error(error)
    );
  }

  rejectPhoto(photoId: number) {
    this.alertify.confirm(
      'Reject Photo',
      'Do you want to delete this photo ?',
      () => {
        this.adminService.rejectPhoto(photoId).subscribe(
          () => {
            this.loadData();
          },
          error => this.alertify.error(error)
        );
      }
    );
  }
}
