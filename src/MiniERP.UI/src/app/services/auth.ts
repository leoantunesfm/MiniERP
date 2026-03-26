import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface RegisterTenantRequest {
  razaoSocial: string;
  nomeFantasia: string;
  cnpj: string;
  nome: string;
  email: string;
  senha: string;
}

export interface RegisterTenantResponse {
  empresaId: string;
  usuarioId: string;
  mensagem: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5209/api'; 

  constructor(private http: HttpClient) { }

  registerTenant(data: RegisterTenantRequest): Observable<RegisterTenantResponse> {
    return this.http.post<RegisterTenantResponse>(`${this.apiUrl}/Tenants/register`, data);
  }
}