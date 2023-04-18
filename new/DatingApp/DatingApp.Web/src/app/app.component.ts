import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users: any;

  constructor(private http: HttpClient, private accountService: AccountService) { }

  ngOnInit(): void {
    this.getUser();
    this.setCurrentUser();
  }

  getUser(){
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: reponse => this.users = reponse,
      error: error => console.log(error),
      complete: () => console.log('request completed')
    });
  }

  setCurrentUser() {
    const userStr = localStorage.getItem('user');
    if(!userStr) return;
    const user: User = JSON.parse(userStr);
    this.accountService.setCurrentUser(user);
  }
}
