import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UsersComponent } from './users';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

describe('UsersComponent', () => {
  let component: UsersComponent;
  let fixture: ComponentFixture<UsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsersComponent],
      providers: [
        provideHttpClient(),
        provideRouter([])
      ]
    }).compileComponents();
    
    fixture = TestBed.createComponent(UsersComponent);
    component = fixture.componentInstance;
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });
});