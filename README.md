Projeto

Este projeto é uma API REST para gerenciamento de uma barbearia, criada com foco em autenticação via JWT, roles (Funcionário / Cliente), Documentado via Swagger.

A aplicação está estruturada para permitir:

Cadastro e login de usuários (Clientes e Funcionários)

Proteção de rotas por permissão (roles)

Registro de clientes, serviços e agendamentos

Documentação interativa com Swagger para testar facilmente os endpoints

🧰 Tecnologias Utilizadas

ASP.NET Core	Framework da API

Entity Framework Core	ORM para o banco de dados

SQL Server Local	Banco de dados usado no ambiente local

ASP.NET Identity	Gerenciamento de usuários e roles

JWT (JSON Web Token)	Autenticação e Autorizações

Swagger (Swashbuckle)	Documentação e interface de teste da API

C#	Linguagem da aplicação


🚀 Como Rodar o Projeto Localmente

1️⃣ Pré-requisitos

Visual Studio 2022 (ou VS Code)

.NET 8.0 SDK (ou aquilo que o projeto usa)

SQL Server (LocalDB ou Express)

No appsettings.json, configure a sua connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BarbeariaDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}

2️⃣ Rodando

No terminal, navegue até a pasta:

cd API/SistemaBarbearia
dotnet run


A aplicação será iniciada normalmente.

3️⃣ Acessando o Swagger

Abra no navegador:

https://localhost:<porta>/swagger


Você verá a interface interativa onde pode testar todos os endpoints da API.

🔐 Autenticação & Controle de Acesso

Para Registrar um usuário (Cliente ou Funcionário):
POST /api/Auth/register
Envie JSON:

{
  "email": "usuario@teste.com",
  "password": "Senha123!",
  "nomeCompleto": "Nome do Usuário",
  "role": "Cliente"            // ou "Funcionario"
}


Para Login:
POST /api/Auth/login
Envie:

{
  "email": "usuario@teste.com",
  "password": "Senha123!"
}


Você receberá um token JWT. Clique no botão Authorize no Swagger, cole:

Bearer SEU_TOKEN_AQUI


A partir disso, rotas protegidas como POST /api/Agendamentos só funcionam se você for Funcionario ou estiver com role correta.

📝 Observações Importantes

Embora a API rode por HTTPS local, certificados auto-assinados podem gerar alerta no navegador — isso é normal para desenvolvimento.

Se você for levar pra produção, configure certificado válido e ambiente adequado.

A documentação dos endpoints está disponível via Swagger, então você pode ver todos os recursos, parâmetros esperados, responses etc.

🚧 Melhorias Futuras

Implementar envio de notificações (SMS ou e-mail) quando um agendamento for criado.

Adicionar paginização e filtros nos endpoints GET.

Construir um frontend (Vue ou React) para consumir essa API.

Migrar para ambiente de produção em Cloud (Azure, AWS, DigitalOcean) com CI/CD.

👤 Autor

Vinícius Froggel
GitHub: https://github.com/ViniciusFroggel
