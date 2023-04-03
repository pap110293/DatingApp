import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../Members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class ReventUnsaveChanges implements CanDeactivate<MemberEditComponent> {
  constructor(
  ) {}

  canDeactivate(
    component: MemberEditComponent,
  ): boolean | import('rxjs').Observable<boolean> | Promise<boolean> {
    if (component.editForm.dirty) {
      return confirm(
        'Are you sure you want to continue? any unsaved changes will be lost'
      );
    }
    return true;
  }
}
