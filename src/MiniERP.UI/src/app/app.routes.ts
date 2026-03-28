import { Routes } from '@angular/router';
import { OnboardingComponent } from './pages/onboarding/onboarding';
import { LoginComponent } from './pages/login/login';
import { DashboardComponent } from './pages/dashboard/dashboard';
import { authGuard } from './guards/auth-guard';
import { guestGuard } from './guards/guest-guard';
import { ConfirmEmailComponent } from './pages/confirm-email/confirm-email';
import { CompleteRegistration } from './pages/complete-registration/complete-registration';

export const routes: Routes = [
  { 
    path: 'login', 
    component: LoginComponent,
    canActivate: [guestGuard] 
  },
  { 
    path: 'onboarding', 
    component: OnboardingComponent,
    canActivate: [guestGuard] 
  },
  { 
    path: 'confirm-email', 
    component: ConfirmEmailComponent,
    canActivate: [guestGuard] 
  },
  { 
    path: 'complete-registration', 
    component: CompleteRegistration,
    canActivate: [authGuard]
  },
  { 
    path: 'dashboard', 
    component: DashboardComponent,
    canActivate: [authGuard] 
  },
  { path: '', redirectTo: '/login', pathMatch: 'full' }
];