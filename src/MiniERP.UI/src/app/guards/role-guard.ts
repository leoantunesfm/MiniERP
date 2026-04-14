import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthorizationService } from '../services/authorization';

export const roleGuard: CanActivateFn = (route, state) => {
  const authZService = inject(AuthorizationService);
  const router = inject(Router);

  const expectedRoles = route.data['roles'] as string[];

  if (!expectedRoles || expectedRoles.length === 0) {
    return true;
  }

  if (authZService.hasRole(expectedRoles)) {
    return true;
  }

  return router.createUrlTree(['/dashboard']);
};