import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-confirm-email',
  template: `
    <div class="min-h-screen flex items-center justify-center bg-base-200 p-4">
      <div class="card w-full max-w-md bg-base-100 shadow-xl text-center p-8">
        <h2 class="text-2xl font-bold mb-4">Confirmação de E-mail</h2>
        
        <div *@if="isLoading" class="loading loading-spinner loading-lg text-primary"></div>
        
        <div *@if="!isLoading && errorMessage" class="text-error">
          <p>{{ errorMessage }}</p>
        </div>

        <div *@if="!isLoading && successMessage" class="text-success">
          <p>{{ successMessage }}</p>
          <button class="btn btn-primary mt-6" (click)="goToLogin()">Ir para o Login</button>
        </div>
      </div>
    </div>
  `,
  standalone: true,
  imports: [/* CommonModule se necessário */]
})
export class ConfirmEmailComponent implements OnInit {
  isLoading = true;
  errorMessage = '';
  successMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit() {
    const token = this.route.snapshot.queryParamMap.get('token');
    
    if (!token) {
      this.errorMessage = 'Token inválido ou não encontrado.';
      this.isLoading = false;
      return;
    }

    this.authService.confirmEmail(token).subscribe({
      next: (res) => {
        this.successMessage = res.mensagem;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.detail || 'Erro ao confirmar o e-mail. O link pode ter expirado.';
        this.isLoading = false;
      }
    });
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}