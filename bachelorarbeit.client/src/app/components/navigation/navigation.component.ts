import { Component } from '@angular/core';
import { RouterModule, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/authentication-service.service';

@Component({
  selector: 'app-navigation',
  standalone: true,
  imports: [RouterModule, RouterLink, RouterLinkActive],
  templateUrl: './navigation.component.html',
  styleUrl: './navigation.component.css',
})
export class NavigationComponent {

  constructor( private readonly authService: AuthService){}

  logout(){
    this.authService.logout();
  }
}
