import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { AuthService, LoginRequest, LoginResponse } from './auth'; 

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AuthService,
        provideHttpClient(),
        provideHttpClientTesting() 
      ]
    });
    
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('deve ser criado (instanciado corretamente)', () => {
    expect(service).toBeTruthy();
  });

  it('deve fazer um POST para o endpoint de login e retornar o Token', () => {
    const loginRequest: LoginRequest = { email: 'teste@teste.com', senha: '123' };
    const mockResponse: LoginResponse = { 
      token: 'token_falso_123', 
      usuarioId: '123', 
      empresaId: '456', 
      nome: 'Usuário Teste' 
    };

    service.login(loginRequest).subscribe(response => {
      expect(response.token).toBe('token_falso_123');
      expect(response.nome).toBe('Usuário Teste');
    });

    const req = httpMock.expectOne('http://localhost:5209/api/Auth/login');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(loginRequest);

    req.flush(mockResponse);
  });
});