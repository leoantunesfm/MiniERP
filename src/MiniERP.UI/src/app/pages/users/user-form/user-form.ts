import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { UserService } from '../../../services/user';
import { ProfileResponse } from '../../../services/user';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './user-form.html'
})
export class UserFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private router = inject(Router);

  perfis: ProfileResponse[] = [];
  loading = false;

  form = this.fb.nonNullable.group({
    nome: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    perfilId: ['', [Validators.required]]
  });

  ngOnInit() {
    this.userService.getProfiles().subscribe({
      next: (data) => this.perfis = data,
      error: (err) => console.error('Erro ao carregar perfis', err)
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.userService.registerUser(this.form.getRawValue()).subscribe({
      next: () => {
        alert('Usuário cadastrado com sucesso! A senha temporária foi enviada para o e-mail dele.');
        this.router.navigate(['/dashboard/users']);
      },
      error: (err) => {
        alert(err.error?.message || 'Erro ao cadastrar usuário.');
        this.loading = false;
      }
    });
  }
}