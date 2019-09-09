import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() registerCancel = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.registerForm = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.minLength(4), Validators.maxLength(8), Validators.required]),
      confirmPassword: new FormControl('', Validators.required)
    }, this.passwordMatchValidator);
  }

  passwordMatchValidator(g: FormGroup){
    return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch': true};
  }

  register() {
    // this.authService.register(this.model).subscribe(
    //   resutl => {
    //     this.alertify.success('registration successful');
    //   },
    //   error => {
    //     this.alertify.error(error);
    //   });
    console.log(this.registerForm.value);
  }

  cancel() {
    this.registerCancel.emit(false);
  }
}
