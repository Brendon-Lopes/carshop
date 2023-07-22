# Carshop

# Índice

- [Carshop](#carshop)
- [Índice](#índice)
- [Descrição](#descrição)
- [Principais tecnologias utilizadas](#principais-tecnologias-utilizadas)
- [Como executar o projeto](#como-executar-o-projeto)
  - [Pré-requisitos](#pré-requisitos)
  - [Como rodar a aplicação](#como-rodar-a-aplicação)
  - [Login administrativo](#login-administrativo)
- [Como executar os testes](#como-executar-os-testes)
  - [Backend](#backend)
  - [Frontend](#frontend)

# Descrição

Sistema de catálogo de carros a venda. Com o login administrativo é possível cadastrar, editar e excluir carros do sistema.

O Backend foi desenvolvido seguindo a arquitetura DDD (Domain Driven Design), SOLID e Clean Code. Visando a escalabilidade, manutenibilidade e testabilidade do sistema.

O Frontend foi desenvolvido tendo em mente boas práticas, componentes reutilizáveis e experiência do usuário.

# Principais tecnologias utilizadas

<strong>Backend:</strong> C#, ASP.NET Core 6.0, Entity Framework Core 6.0, BCrypt, Docker, XUnit, FluentAssertions.

<strong>Frontend:</strong> React + Vite + TypeScript, Tailwind CSS, Axios, React-cookie, React-router-dom, Yup, Docker, Vitest.

<strong>Banco de dados:</strong> SQL Server. (Dockerizado para facilitar a execução do projeto).

# Como executar o projeto

## Pré-requisitos

- Docker
- Docker Compose
- Git
- .NET 6.0 (Para rodar os testes do backend)
- Node.js (Para rodar os testes do frontend)

## Como rodar a aplicação

Clonar o repositório:

```bash
git clone git@github.com:Brendon-Lopes/carshop.git
```

Entrar na pasta do projeto:

```bash
cd carshop
```

Rodar com docker compose (Pode demorar um pouco pra iniciar):

```bash
docker-compose up -d
```

O projeto estará disponível em http://localhost:3000

A API estará disponível em http://localhost:5000

A documentação da API estará disponível em http://localhost:5000/swagger

## Login administrativo

Email:

```bash
admin@mail.com
```

Senha:

```bash
123456
```

# Como executar os testes

## Backend

Entrar na pasta do projeto:

```bash
cd server
```

Executar os testes:

```bash
dotnet test
```

## Frontend

Entrar na pasta do projeto:

```bash
cd client
```

Instalar as dependências:

```bash
npm install
```

Executar os testes:

```bash
npm test
```
