import { Routes } from '@angular/router';
import { OnboardingComponent } from './pages/onboarding/onboarding';
import { LoginComponent } from './pages/login/login';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'onboarding', component: OnboardingComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' }
];
