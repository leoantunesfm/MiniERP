CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE TABLE "Empresas" (
        "Id" uuid NOT NULL,
        "RazaoSocial" character varying(200),
        "NomeFantasia" character varying(200),
        "Cnpj" character varying(14) NOT NULL,
        "DataCadastro" timestamp with time zone NOT NULL,
        "Status" integer NOT NULL,
        "Telefone" text,
        "Cep" character varying(8),
        "Logradouro" text,
        "Numero" text,
        "Complemento" text,
        "Bairro" text,
        "Municipio" text,
        "Uf" text,
        CONSTRAINT "PK_Empresas" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE TABLE "Perfis" (
        "Id" uuid NOT NULL,
        "Nome" character varying(50) NOT NULL,
        "Descricao" character varying(250) NOT NULL,
        CONSTRAINT "PK_Perfis" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE TABLE "DocumentosEmpresas" (
        "Id" uuid NOT NULL,
        "EmpresaId" uuid NOT NULL,
        "NomeArquivo" character varying(255) NOT NULL,
        "S3Path" character varying(1000) NOT NULL,
        "DataUpload" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_DocumentosEmpresas" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_DocumentosEmpresas_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE TABLE "Usuarios" (
        "Id" uuid NOT NULL,
        "EmpresaId" uuid NOT NULL,
        "Nome" character varying(150) NOT NULL,
        "Email" character varying(150) NOT NULL,
        "SenhaHash" text NOT NULL,
        "Ativo" boolean NOT NULL,
        CONSTRAINT "PK_Usuarios" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Usuarios_Empresas_EmpresaId" FOREIGN KEY ("EmpresaId") REFERENCES "Empresas" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE TABLE "UsuarioPerfis" (
        "UsuarioId" uuid NOT NULL,
        "PerfilId" uuid NOT NULL,
        CONSTRAINT "PK_UsuarioPerfis" PRIMARY KEY ("UsuarioId", "PerfilId"),
        CONSTRAINT "FK_UsuarioPerfis_Perfis_PerfilId" FOREIGN KEY ("PerfilId") REFERENCES "Perfis" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_UsuarioPerfis_Usuarios_UsuarioId" FOREIGN KEY ("UsuarioId") REFERENCES "Usuarios" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    INSERT INTO "Perfis" ("Id", "Descricao", "Nome")
    VALUES ('f0000000-0000-0000-0000-000000000001', 'Administrador do Sistema', 'Admin');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE INDEX "IX_DocumentosEmpresas_EmpresaId" ON "DocumentosEmpresas" ("EmpresaId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Empresas_Cnpj" ON "Empresas" ("Cnpj");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE INDEX "IX_UsuarioPerfis_PerfilId" ON "UsuarioPerfis" ("PerfilId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Usuarios_Email" ON "Usuarios" ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    CREATE INDEX "IX_Usuarios_EmpresaId" ON "Usuarios" ("EmpresaId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260327230459_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260327230459_InitialCreate', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260328040749_AddTokenConfirmacaoEmail') THEN
    ALTER TABLE "Empresas" ADD "TokenConfirmacaoEmail" text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260328040749_AddTokenConfirmacaoEmail') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260328040749_AddTokenConfirmacaoEmail', '10.0.5');
    END IF;
END $EF$;
COMMIT;

