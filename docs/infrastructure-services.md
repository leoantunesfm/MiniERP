# Serviços de Infraestrutura

Para suportar operações assíncronas e armazenamento de arquivos compatível com a nuvem, o MiniERP utiliza ferramentas de infraestrutura consolidadas rodando em containers Docker.

## 1. Mensageria: RabbitMQ
Responsável por desacoplar processos demorados (como envio de e-mails ou processamento de relatórios pesados) do ciclo de vida da requisição HTTP (Request/Response).
* **Imagem Docker:** `rabbitmq:3-management` (inclui painel visual).
* **Filas Iniciais:** `email-notifications`.
* **Implementação .NET:** Utilizaremos a biblioteca nativa `RabbitMQ.Client` ou `MassTransit` para facilitar o roteamento de mensagens na camada de *Infrastructure*.

## 2. Storage de Arquivos: MinIO
Servidor de armazenamento de objetos (Object Storage) de alto desempenho, 100% compatível com a API do Amazon S3. 
* **Propósito:** Armazenar uploads de clientes (documentos de onboarding, notas fiscais, avatares) sem sobrecarregar o banco de dados.
* **Imagem Docker:** `minio/minio`.
* **Implementação .NET:** A aplicação utilizará o pacote oficial `AWSSDK.S3`. No ambiente local, o SDK apontará para o container do MinIO. Em produção (AWS), apontará para o S3 verdadeiro apenas mudando variáveis de ambiente, garantindo portabilidade absoluta ("Cloud Native").