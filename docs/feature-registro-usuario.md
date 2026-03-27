# Feature: Registro de Novo Usuário (Onboarding Multitenant)

## Descrição
Fluxo de entrada de um novo cliente na plataforma. O processo foi dividido em etapas para garantir a validação de identidade (e-mail) e coletar dados precisos de forma automatizada (integração ReceitaWS e upload de documentos).

## Fluxo de Estados (Tenant Status)
Uma empresa/usuário passará pelos seguintes status:
1. `AguardandoConfirmacaoEmail`: Conta criada, mas sem acesso liberado.
2. `AguardandoDadosCompletos`: E-mail validado. O usuário pode fazer login, mas fica travado na tela de conclusão de cadastro.
3. `Ativo`: Todos os dados e documentos foram fornecidos. Acesso total liberado.

## 1. Etapa 1: Registro Inicial e Validação de E-mail
* **Front-end:** Formulário simples contendo apenas **CNPJ**, Nome do Administrador, E-mail e Senha.
* **Back-end:** * Cria a empresa com status `AguardandoConfirmacaoEmail`.
  * Publica uma mensagem na fila do RabbitMQ (`email-validation-queue`) contendo o e-mail, nome e um *Token de Validação* (Guid exclusivo ou JWT temporário).
* **Worker (Mensageria):** Um serviço em *background* consome a fila e faz o disparo SMTP do e-mail com o link de confirmação.

## 2. Etapa 2: Conclusão de Cadastro (Enriquecimento de Dados)
* **API Externa:** Após o usuário clicar no link do e-mail, ele faz login e cai na tela de conclusão. O Back-end consulta a API pública `https://receitaws.com.br/v1/cnpj/{cnpj}`.
* **Front-end:** Apresenta um formulário pré-preenchido com Razão Social, Nome Fantasia, CEP, Logradouro, Número, Complemento, Bairro, Município, UF e Telefone. O usuário pode editar os campos.
* **Upload de Documentos:** Na mesma tela, o usuário anexa arquivos comprobatórios (ex: Contrato Social). O Back-end envia esses arquivos para um *Bucket* no MinIO (S3 Compatible).
* **Conclusão:** O Back-end salva a URL/Path do documento no banco, atualiza os dados da empresa e muda o status para `Ativo`.

## 3. Banco de Dados (Alterações Previstas)
* **Tabela `Empresas`:** Adição dos campos de Endereço, Telefone, e campo `Status` (Enum).
* **Tabela `DocumentosEmpresa`:** Nova tabela para registrar os metadados dos arquivos anexados (Id, EmpresaId, NomeArquivo, S3Path, DataUpload).