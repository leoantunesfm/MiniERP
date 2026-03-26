import { Routes } from '@angular/router';
import { OnboardingComponent } from './pages/onboarding/onboarding';

export const routes: Routes = [
    { path: 'onboarding', component: OnboardingComponent },
    { path: '', redirectTo: '/onboarding', pathMatch: 'full' }
];
