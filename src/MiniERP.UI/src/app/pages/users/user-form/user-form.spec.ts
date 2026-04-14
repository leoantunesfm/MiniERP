import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserFormComponent } from './user-form';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

describe('UserFormComponent', () => {
  let component: UserFormComponent;
  let fixture: ComponentFixture<UserFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserFormComponent],
      providers: [
        provideHttpClient(),
        provideRouter([]) // Mock de rotas
      ]
    }).compileComponents();
    
    fixture = TestBed.createComponent(UserFormComponent);
    component = fixture.componentInstance;
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });
});