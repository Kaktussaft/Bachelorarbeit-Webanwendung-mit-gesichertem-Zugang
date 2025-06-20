import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FooterComponent } from '../../components/footer/footer.component';
import { NavigationComponent } from '../../components/navigation/navigation.component';


@Component({
  selector: 'app-home',
  imports: [CommonModule, FooterComponent, NavigationComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {}
