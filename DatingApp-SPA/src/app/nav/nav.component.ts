import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  modelLogin: any = {};
  photoUrl: string;

  constructor(
    public authService: AuthService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(url => this.photoUrl = url);
  }

  login() {
    this.authService.login(this.modelLogin).subscribe(
      () => {
        this.alertify.success('logged in successfully');
        this.router.navigate(['/members']);
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  isLoggedIn() {
    return this.authService.isLoggedIn();
  }

  logout() {
    this.modelLogin = {};
    this.authService.logout();
    this.alertify.message('logged out');
    this.router.navigate(['/']);
  }
}
