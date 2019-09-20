import { BrowserModule, HammerGestureConfig, HAMMER_GESTURE_CONFIG } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from 'ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TimeAgoPipe } from 'time-ago-pipe';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { MemberListComponent } from './Members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { MemberCardComponent } from './Members/member-card/member-card.component';
import { environment } from 'src/environments/environment';
import { MemberDetailComponent } from './Members/member-detail/member-detail.component';
import { PhotoEditorComponent } from './Members/photo-editor/photo-editor.component';
import { AuthGuard } from './_guards/auth.guard';
import { UserService } from './_services/user.service';
import { AlertifyService } from './_services/alertify.service';
import { MemberEditComponent } from './Members/member-edit/member-edit.component';
import { MemeberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemeberEditResolver } from './_resolvers/member-edit.resolver';
import { MemeberListResolver } from './_resolvers/member-list.resolver';
import { ReventUnsaveChanges } from './_guards/revent-insave-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/Messages.resolver';
import { MemberMessagesComponent } from './Members/member-messages/member-messages.component';

export function tokenGetter() {
  return localStorage.getItem(environment.tokenLocalStoreKey);
}

export class CustomHammerConfig extends HammerGestureConfig  {
  overrides = {
      pinch: { enable: false },
      rotate: { enable: false }
  };
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    MemberCardComponent,
    MemberDetailComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TimeAgoPipe,
    MemberMessagesComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    FileUploadModule,
    ButtonsModule.forRoot(),
    PaginationModule.forRoot(),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => tokenGetter(),
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth']
      }
    }),
    TabsModule.forRoot(),
    NgxGalleryModule,
    ReactiveFormsModule
  ],
  providers: [
    { provide: HAMMER_GESTURE_CONFIG, useClass: CustomHammerConfig },
    AuthService,
    ErrorInterceptorProvider,
    AlertifyService,
    AuthGuard,
    UserService,
    MemeberDetailResolver,
    MemeberEditResolver,
    MemeberListResolver,
    MessagesResolver,
    ReventUnsaveChanges,
    ListsResolver
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
