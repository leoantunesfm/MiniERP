import { Component } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HasRoleDirective } from './has-role';
import { AuthorizationService } from '../services/authorization';

@Component({
  standalone: true,
  imports: [HasRoleDirective],
  template: `<div *appHasRole="['Admin']">Conteúdo Protegido</div>`
})
class DummyComponent {}

describe('HasRoleDirective', () => {
  let fixture: ComponentFixture<DummyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DummyComponent],
      providers: [
        { provide: AuthorizationService, useValue: { hasRole: () => true } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DummyComponent);
  });

  it('deve criar a diretiva com sucesso', () => {
    expect(fixture.componentInstance).toBeTruthy();
  });
});