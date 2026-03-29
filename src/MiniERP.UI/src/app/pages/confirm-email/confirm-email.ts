import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.html',
  standalone: true
})
export class ConfirmEmailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  isLoading = true;
  errorMessage = '';
  successMessage = '';

  ngOnInit() {
    const token = this.route.snapshot.queryParamMap.get('token');
    
    if (!token) {
      this.errorMessage = 'Token inválido ou link mal formatado.';
      this.isLoading = false;
      return;
    }

    this.authService.confirmEmail(token).subscribe({
      next: (res: any) => {
        
        this.successMessage = res?.mensagem || res?.Mensagem || 'E-mail confirmado com sucesso!';
        this.isLoading = false;
        
        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('3. Encomenda chegou com ERRO!', err);
        
        this.errorMessage = err.error?.detail || 'Erro ao confirmar o e-mail. O link pode já ter sido usado ou expirado.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }
}