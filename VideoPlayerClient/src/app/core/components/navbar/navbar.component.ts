import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../_services/account.service';
import { CommonModule, NgIf } from '@angular/common';
import { RegisterComponent } from '../register/register.component';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedModule } from '../../_modules/shared.module';
import { LoginUser, User } from '../../_models/user';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    FormsModule,
    NgIf,
    RegisterComponent,
    RouterLink,
    RouterLinkActive,
    SharedModule,
    CommonModule,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent implements OnInit {
  @Input() user: User | undefined;
  
  form: LoginUser = {
    name: '',
    password: '',
  };

  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  login() {
    const model = {
      name: this.form.name,
      password: this.form.password,
    };

    this.accountService.login(model).subscribe({
      next: () => this.router.navigateByUrl('/videos'),
      error: (error) => {
        this.toastr.error(error[0].message);
      },
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
