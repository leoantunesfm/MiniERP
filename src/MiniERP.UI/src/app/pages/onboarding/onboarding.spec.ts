import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http'; 
import { provideHttpClientTesting } from '@angular/common/http/testing'; 

import { OnboardingComponent } from './onboarding';

describe('OnboardingComponent', () => {
  let component: OnboardingComponent;
  let fixture: ComponentFixture<OnboardingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OnboardingComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()] // Adicionar os providers
    }).compileComponents();

    fixture = TestBed.createComponent(OnboardingComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});