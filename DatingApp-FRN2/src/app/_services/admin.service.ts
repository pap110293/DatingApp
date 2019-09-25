import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getUserWithRole() {
    return this.http.get(this.baseUrl + 'admin/userWithRoles');
  }

  getAllRoles() {
    return this.http.get(this.baseUrl + 'admin/allRoles');
  }

  editRoles(user: User, roleNames: string[]) {
    return this.http.post(this.baseUrl + 'admin/' + user.userName + '/roles' , { roleNames });
  }
}
