import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { RegisterUser } from '../../_models/user';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  form: RegisterUser = {
    name: '',
    password: '',
  };

  constructor(
    private accountService: AccountService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  register() {
    this.accountService.register(this.form).subscribe({
      next: () => {
        this.cancel();
      },
      error: (error) => this.toastr.error(error.error),
      // error: (error) => console.log(error),
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
