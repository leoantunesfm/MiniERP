import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { Router, RouterModule } from '@angular/router';


@Component({
  selector: 'app-onboarding',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterModule], 
  templateUrl: './onboarding.html',
  styleUrl: './onboarding.scss'
})
export class OnboardingComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  onboardingForm: FormGroup = this.fb.group({
    razaoSocial: ['', Validators.required],
    nomeFantasia: ['', Validators.required],
    cnpj: ['', [Validators.required, Validators.minLength(14), Validators.maxLength(14)]],
    nome: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    senha: ['', [Validators.required, Validators.minLength(6)]]
  });

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  onSubmit() {
    if (this.onboardingForm.invalid) {
      this.onboardingForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formData = this.onboardingForm.value;

    this.authService.registerTenant(formData).subscribe({
      next: (response) => {
        this.successMessage = response.mensagem;
        this.isLoading = false;
        this.onboardingForm.reset();
        
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);        
      },
      error: (err) => {
        this.errorMessage = err.error?.Erro || 'Ocorreu um erro ao processar o cadastro.';
        this.isLoading = false;
      }
    });
  }
}