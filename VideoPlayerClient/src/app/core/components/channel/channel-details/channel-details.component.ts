import { Component, OnInit } from '@angular/core';
import { User } from '../../../_models/user';
import { UsersService } from '../../../_services/users.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-channel-details',
  standalone: true,
  imports: [],
  templateUrl: './channel-details.component.html',
  styleUrl: './channel-details.component.scss',
})
export class ChannelDetailsComponent implements OnInit {
  user: User | undefined;

  constructor(
    private usersService: UsersService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    const id = this.route.snapshot.paramMap.get('id');
    console.log('id', id);
    if (!id) return;
    this.usersService.getUser(id).subscribe({
      next: (user) => (this.user = user),
    });
  }
}
