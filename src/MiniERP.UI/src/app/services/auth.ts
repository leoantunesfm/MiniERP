import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface RegisterTenantRequest {
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

export interface ConsultaCnpjResponseDto {
  nome?: string;
  fantasia?: string;
  logradouro?: string;
  numero?: string;
  complemento?: string;
  bairro?: string;
  municipio?: string;
  uf?: string;
  cep?: string;
  telefone?: string;
}

export interface LoginRequest {
  email: string;
  senha: string;
}

export interface LoginResponse {
  token: string;
  usuarioId: string;
  empresaId: string;
  nome: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  registerTenant(data: RegisterTenantRequest): Observable<RegisterTenantResponse> {
    return this.http.post<RegisterTenantResponse>(`${this.apiUrl}/Tenants/register`, data);
  }

  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/Auth/login`, data);
  }

  confirmEmail(token: string) {
    return this.http.get<{mensagem: string}>(`${this.apiUrl}/tenants/confirm-email?token=${token}`);
  }

  getCnpjData(cnpj: string) {
    return this.http.get<ConsultaCnpjResponseDto>(`${this.apiUrl}/tenants/cnpj-data/${cnpj}`);
  }

  completeRegistration(formData: FormData) {
    return this.http.post(`${this.apiUrl}/tenants/complete-registration`, formData);
  }

  getTenantById(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/tenants/${id}`);
  }
}