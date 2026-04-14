import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth';
import { HasRoleDirective } from '../../directives/has-role';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, HasRoleDirective],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);
  
  nomeUsuario: string = '';

  ngOnInit() {
    this.nomeUsuario = this.authService.getUserName();
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}