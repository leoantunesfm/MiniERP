import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  private decodeToken(token: string): any {
    try {
      const payload = token.split('.')[1];
      const decoded = atob(payload);
      return JSON.parse(decoded);
    } catch (e) {
      return null;
    }
  }

  getUserRole(): string | null {
    const token = localStorage.getItem('minierp_token'); 
    if (!token) return null;

    const decoded = this.decodeToken(token);
    if (!decoded) return null;

    const roleClaim = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
    return decoded[roleClaim] || decoded.role || null;
  }

  hasRole(allowedRoles: string[]): boolean {
    const userRole = this.getUserRole();
    if (!userRole) return false;
    
    return allowedRoles.includes(userRole);
  }
}