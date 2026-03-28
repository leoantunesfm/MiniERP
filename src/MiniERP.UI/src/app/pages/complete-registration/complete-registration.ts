import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-complete-registration',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './complete-registration.html',
  styleUrl: './complete-registration.scss'
})
export class CompleteRegistration implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  form: FormGroup = this.fb.group({
    cnpjExibicao: [{value: '', disabled: true}],
    razaoSocial: ['', Validators.required],
    nomeFantasia: ['', Validators.required],
    telefone: ['', Validators.required],
    cep: ['', Validators.required],
    logradouro: ['', Validators.required],
    numero: ['', Validators.required],
    complemento: [''],
    bairro: ['', Validators.required],
    municipio: ['', Validators.required],
    uf: ['', [Validators.required, Validators.maxLength(2)]]
  });

  isLoading = false;
  isSearching = true;
  errorMessage = '';
  selectedFiles: File[] = [];
  empresaId: string | null = '';

  ngOnInit(): void {
    this.empresaId = localStorage.getItem('minierp_empresa_id');
    
    if (this.empresaId) {
      this.authService.getTenantById(this.empresaId).subscribe({
        next: (tenant) => {
          this.form.patchValue({ cnpjExibicao: tenant.cnpj });
          this.buscarDadosReceita(tenant.cnpj);
        },
        error: () => {
          this.isSearching = false;
          this.errorMessage = 'Erro ao carregar dados do seu cadastro.';
        }
      });
    }
  }

  buscarDadosReceita(cnpj: string) {
    this.authService.getCnpjData(cnpj).subscribe({
      next: (dados: any) => {
        this.isSearching = false;
        if (dados.nome) {
          this.form.patchValue({
            razaoSocial: dados.nome,
            nomeFantasia: dados.fantasia || dados.nome,
            telefone: dados.telefone,
            cep: dados.cep?.replace(/\D/g, ''),
            logradouro: dados.logradouro,
            numero: dados.numero,
            complemento: dados.complemento,
            bairro: dados.bairro,
            municipio: dados.municipio,
            uf: dados.uf
          });
        }
      },
      error: () => {
        this.isSearching = false;
      }
    });
  }

  onFileSelected(event: any) {
    const files: FileList = event.target.files;
    this.selectedFiles = [];
    if (files) {
      for (let i = 0; i < files.length; i++) {
        this.selectedFiles.push(files[i]);
      }
    }
  }

  onSubmit() {
    if (this.form.invalid || this.selectedFiles.length === 0) {
      this.form.markAllAsTouched();
      this.errorMessage = this.selectedFiles.length === 0 ? 'Anexe pelo menos um documento.' : 'Preencha os campos obrigatórios.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formData = new FormData();
    formData.append('EmpresaId', this.empresaId!);

    Object.keys(this.form.controls).forEach(key => {
      if (key !== 'cnpjExibicao') {
        formData.append(key, this.form.get(key)?.value || '');
      }
    });

    this.selectedFiles.forEach(file => {
      formData.append('documentos', file, file.name);
    });

    this.authService.completeRegistration(formData).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.errorMessage = err.error?.detail || 'Erro ao enviar dados.';
        this.isLoading = false;
      }
    });
  }
}