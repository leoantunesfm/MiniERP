# Planning: Deploy AWS e CI/CD com GitHub Actions

Este documento detalha as fases para migrar o MiniERP do ambiente local (Docker Compose) para uma infraestrutura em nuvem na AWS, implementando um pipeline de CI/CD contínuo e adotando serviços gerenciados para banco de dados, storage e e-mail.

## Domínios e Roteamento
- **Domínio Base:** `fillgaps.com.br`
- **Front-end:** `minierp.fillgaps.com.br`
- **API:** `api.minierp.fillgaps.com.br`

## Fases de Implementação

### Fase 1: Adaptação do Código para Múltiplos Ambientes
O objetivo desta fase é garantir que o sistema rode com MinIO/Mailpit localmente, mas use S3/SES em produção sem alterar o código C#.
- [ ] **E-mail (AWS SES):**
  - O SES fornece credenciais SMTP padrão. O nosso `EmailService` atual já funciona.
  - Ajustar o `appsettings.json` para carregar configurações via variáveis de ambiente no Docker.
- [ ] **Storage (AWS S3):**
  - Ajustar a injeção de dependência do `AmazonS3Client` no C# para que, se não houver um `ServiceURL` customizado (como o do MinIO), ele use a região padrão da AWS.
- [ ] **Docker Compose de Produção:**
  - Criar um `docker-compose.prod.yml` limpo, contendo apenas: `api`, `ui` e `rabbitmq`.
  - Remover `db`, `pgadmin`, `mailpit` e `minio` deste arquivo, pois serão substituídos por serviços gerenciados da AWS.

### Fase 2: Infraestrutura Cloud Base (AWS)
Criação dos serviços gerenciados que armazenam estado (Stateful).
- [ ] **AWS IAM (Identity and Access Management):**
  - Criar um usuário programático (Access Key / Secret Key) para o MiniERP com permissões limitadas ao S3 e SES.
- [ ] **Amazon S3 (Armazenamento):**
  - Criar o bucket `minierp-documentos-prod`.
  - Configurar CORS no bucket para permitir requisições do domínio do front-end (opcional, dependendo de como os arquivos serão lidos).
- [ ] **Amazon SES (E-mail Transacional):**
  - Adicionar e verificar o domínio `fillgaps.com.br` no SES.
  - Inserir os registros CNAME/TXT de DKIM no painel de DNS do Registro.br.
  - Gerar credenciais SMTP do SES.
- [ ] **Amazon RDS (Banco de Dados):**
  - Provisionar uma instância PostgreSQL (Free Tier - db.t3.micro ou t4g.micro).
  - Configurar o Security Group para permitir acesso apenas da instância EC2 que criaremos.

### Fase 3: Preparação do Servidor de Aplicação (AWS EC2)
Criação do ambiente onde os containers vão rodar.
- [ ] **Instância EC2:**
  - Subir uma instância EC2 (Ubuntu 24.04 LTS - Free Tier eligible).
  - Configurar Security Group liberando as portas 80 (HTTP), 443 (HTTPS) e 22 (SSH).
- [ ] **Configuração do Servidor:**
  - Acessar via SSH e instalar o Docker e o plugin Docker Compose.
  - Instalar o Nginx (direto na máquina) para atuar como *Reverse Proxy* e rotear os subdomínios para os containers corretos.
- [ ] **Configuração de SSL/TLS:**
  - Instalar o Certbot (Let's Encrypt) na EC2 para gerar os certificados HTTPS automáticos para `minierp.fillgaps.com.br` e `api.minierp.fillgaps.com.br`.

### Fase 4: Configuração de Repositório de Imagens (AWS ECR)
- [ ] Criar repositório no Amazon ECR para a imagem da API (`minierp/api`).
- [ ] Criar repositório no Amazon ECR para a imagem do Front-end (`minierp/ui`).

### Fase 5: Pipeline CI/CD (GitHub Actions)
Automatizar o processo de deploy. Quando um código for mergeado na branch `main`, o pipeline deve:
- [ ] **Build & Push:**
  - Fazer o checkout do código.
  - Fazer login na AWS usando credenciais configuradas no *GitHub Secrets*.
  - Fazer o build das imagens Docker (API e UI).
  - Subir (Push) as imagens para o Amazon ECR.
- [ ] **Deploy na EC2:**
  - Acessar a EC2 via SSH remoto (usando chave privada no GitHub Secrets).
  - Fazer o pull das novas imagens do ECR.
  - Rodar o comando de atualização: `docker compose -f docker-compose.prod.yml up -d --build`.

### Fase 6: Testes e Validação em Produção
- [ ] Roteamento de DNS: Apontar subdomínios no Registro.br para o IP Público (Elastic IP) da EC2.
- [ ] Executar o fluxo completo de *Onboarding* usando o domínio real.
- [ ] Verificar recebimento do e-mail via Amazon SES.
- [ ] Validar upload de arquivos e persistência no AWS S3 real.