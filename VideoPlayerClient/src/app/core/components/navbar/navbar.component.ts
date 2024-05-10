import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../_services/account.service';
import { CommonModule, NgIf } from '@angular/common';
import { RegisterComponent } from '../register/register.component';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedModule } from '../../_modules/shared.module';

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
  model: any = {};

  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  login() {
    this.accountService.login(this.model).subscribe({
      next: () => this.router.navigateByUrl('/videos'),
      error: (error) => {
        this.toastr.error(error.error.errors[0]?.message), console.log(error);
      },
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
