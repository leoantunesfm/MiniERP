import { TestBed } from '@angular/core/testing';
import { AuthorizationService } from './authorization';

describe('AuthorizationService', () => {
  let service: AuthorizationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthorizationService);
  });

  it('deve ser criado', () => {
    expect(service).toBeTruthy();
  });
});