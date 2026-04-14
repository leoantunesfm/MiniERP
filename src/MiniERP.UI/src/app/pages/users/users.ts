import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserService, UserResponse } from '../../services/user';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './users.html'
})
export class UsersComponent implements OnInit {
  private userService = inject(UserService);
  private cdr = inject(ChangeDetectorRef);
  
  users: UserResponse[] = [];
  loading = true;

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.loading = true;
    this.userService.getUsers().subscribe({
      next: (response: any) => {
        const rawItems = response?.data?.itens || response?.itens || response || [];
        
        this.users = rawItems.map((u: any) => ({
          id: u.id || u.Id,
          nome: u.nome || u.Nome,
          email: u.email || u.Email,
          perfil: typeof u.perfil === 'object' ? u.perfil?.nome : (u.perfil || u.Perfil || 'N/A'),
          perfilId: u.perfilId || u.PerfilId,
          ativo: u.ativo !== undefined ? u.ativo : u.Ativo
        }));

        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  deactivate(id: string) {
    if (confirm('Deseja realmente inativar este usuário?')) {
      this.userService.deactivateUser(id).subscribe({
        next: () => this.loadUsers()
      });
    }
  }
}