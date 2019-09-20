import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { Message } from 'src/app/_models/Message';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() recipientId: number;
  messages: Message[];
  newMessage: any = {};
  constructor(
    private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.loadMessages();
  }

  loadMessages() {
    const currentUserId = +this.authService.getCurrentUserId();
    this.userService
      .getMessageThread(this.authService.getCurrentUserId(), this.recipientId)
      .pipe(
        tap(messages => {
          // tslint:disable-next-line: prefer-for-of
          for (let i = 0; i < messages.length; i++) {
            const element = messages[i];
            if (
              element.isRead === false &&
              element.recipientId === currentUserId
            ) {
              this.userService.markAsRead(currentUserId, element.id);
            }
          }
        })
      )
      .subscribe(
        messages => {
          this.messages = messages;
        },
        error => this.alertify.error(error)
      );
  }

  sendMessage() {
    this.newMessage.recipientId = this.recipientId;
    this.userService
      .sendMessage(this.authService.getCurrentUserId(), this.newMessage)
      .subscribe(
        (message: Message) => {
          this.messages.unshift(message);
          this.newMessage.content = '';
        },
        error => {
          this.alertify.error(error);
        }
      );
  }
}
