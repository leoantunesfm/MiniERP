import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent implements OnInit {
  private router = inject(Router);
  
  nomeUsuario: string = '';

  ngOnInit() {
    this.nomeUsuario = localStorage.getItem('minierp_user_nome') || 'Usuário';
  }

  logout() {
    localStorage.removeItem('minierp_token');
    localStorage.removeItem('minierp_user_nome');
    
    this.router.navigate(['/login']);
  }
}