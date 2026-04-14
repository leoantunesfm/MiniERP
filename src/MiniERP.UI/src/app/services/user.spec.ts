import { TestBed } from '@angular/core/testing';
import { UserService } from './user';
import { provideHttpClient } from '@angular/common/http';

describe('UserService', () => {
  let service: UserService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient()
      ]
    });
    service = TestBed.inject(UserService);
  });

  it('deve ser criado', () => {
    expect(service).toBeTruthy();
  });
});