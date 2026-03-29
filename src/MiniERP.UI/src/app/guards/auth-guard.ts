import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const token = localStorage.getItem('minierp_token');
  const tenantStatus = localStorage.getItem('minierp_tenant_status');

  if (!token) {
    router.navigate(['/login']);
    return false;
  }

  if (tenantStatus === 'AguardandoDadosCompletos' && state.url !== '/complete-registration') {
    router.navigate(['/complete-registration']);
    return false;
  }

  if (tenantStatus === 'Ativo' && state.url === '/complete-registration') {
    router.navigate(['/dashboard']);
    return false;
  }

  return true;
};