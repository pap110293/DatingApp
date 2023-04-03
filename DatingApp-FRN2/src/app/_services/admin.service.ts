import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '../_models/user';
import { PaginatedResutl } from '../_models/pagination';
import { Photo } from '../_models/photo';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

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
    return this.http.post(this.baseUrl + 'admin/' + user.userName + '/roles', {
      roleNames
    });
  }

  getWatingForApprovePhotos(
    page?,
    itemsPerPage?
  ): Observable<PaginatedResutl<Photo[]>> {
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    const paginatedResutl: PaginatedResutl<Photo[]> = new PaginatedResutl<
      Photo[]
    >();

    return this.http
      .get<PaginatedResutl<Photo[]>>(this.baseUrl + 'admin/photos', {
        observe: 'response',
        params
      })
      .pipe(
        map((res: any) => {
          paginatedResutl.result = res.body;
          if (res.headers.get('Pagination') != null) {
            paginatedResutl.pagination = JSON.parse(
              res.headers.get('Pagination')
            );
          }
          return paginatedResutl;
        })
      );
  }

  approvePhoto(photoId) {
    return this.http.post(this.baseUrl + 'admin/photos/' + photoId, {});
  }

  rejectPhoto(photoId) {
    return this.http.delete(this.baseUrl + 'admin/photos/' + photoId);
  }
}
