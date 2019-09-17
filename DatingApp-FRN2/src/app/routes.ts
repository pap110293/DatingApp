import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './Members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './Members/member-detail/member-detail.component';
import { MemberEditComponent } from './Members/member-edit/member-edit.component';
import { MemeberEditResolver } from './_resolvers/member-edit.resolver';
import { MemeberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemeberListResolver } from './_resolvers/member-list.resolver';
import { ReventUnsaveChanges } from './_guards/revent-insave-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: 'members',
        component: MemberListComponent,
        resolve: { users: MemeberListResolver }
      },
      {
        path: 'members/edit',
        component: MemberEditComponent,
        resolve: { user: MemeberEditResolver },
        canDeactivate: [ReventUnsaveChanges]
      },
      {
        path: 'members/:id',
        component: MemberDetailComponent,
        resolve: { user: MemeberDetailResolver }
      },
      { path: 'messages', component: MessagesComponent },
      {
        path: 'lists',
        component: ListsComponent,
        resolve: { users: ListsResolver }
      }
    ]
  },

  { path: '**', redirectTo: '', pathMatch: 'full' } //   wildcard route
];
