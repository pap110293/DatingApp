import { Injectable } from '@angular/core';
import alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {
  constructor() {}

  confirm(title: string,  message: string, okCallBack: () => any) {
    alertify.confirm(message, (e: any) => {
      if (e) {
        okCallBack();
      }
    });
  }

  success(message: string) {
    alertify.success(message);
  }

  error(message: string) {
    alertify.error(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }

  message(message: string) {
    alertify.message(message);
  }
}
