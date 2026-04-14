import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface UserResponse {
  id: string;
  nome: string;
  email: string;
  perfil: string;
  perfilId: string;
  ativo: boolean;
}

export interface ProfileResponse {
  id: string;
  nome: string;
  descricao: string;
}

export interface RegisterUserRequest {
  nome: string;
  email: string;
  perfilId: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/users`;
  private profilesUrl = `${environment.apiUrl}/profiles`;

  getUsers(): Observable<UserResponse[]> {
    return this.http.get<UserResponse[]>(this.apiUrl);
  }

  deactivateUser(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  
  getProfiles(): Observable<ProfileResponse[]> {
    return this.http.get<ProfileResponse[]>(this.profilesUrl);
  }

  registerUser(data: RegisterUserRequest): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }
}