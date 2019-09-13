import { User } from './user';

export interface Pagination {
  currentPage: number;
  itemPerPage: number;
  totalItems: number;
  totalPages: number;
}

export class PaginatedResutl<T> {
  result: T;
  pagination: Pagination;
  paginatedResutl: PaginatedResutl<User[]>;
}
