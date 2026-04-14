import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { DashboardComponent } from './dashboard';
import { AuthService } from '../../services/auth';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardComponent],
      providers: [
        
        provideRouter([]), 

        {
          provide: AuthService,
          useValue: {
            getUserName: () => 'Usuário de Teste',
            logout: () => {}
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('deve criar o componente com sucesso', () => {
    expect(component).toBeTruthy();
  });
});