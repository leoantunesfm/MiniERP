import { Routes } from '@angular/router';
import { OnboardingComponent } from './pages/onboarding/onboarding';
import { LoginComponent } from './pages/login/login';
import { DashboardComponent } from './pages/dashboard/dashboard';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'onboarding', component: OnboardingComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' }
];
