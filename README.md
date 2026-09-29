# Painel de Manutenção de Produtos

Sistema web para cadastro, consulta, edição e desativação de produtos por meio de uma interface, com histórico de alterações. O objetivo é permitir que manutenções que antes exigiam chamadas diretas à API ou alterações manuais no banco sejam feitas por uma tela.

> **Status:** em desenvolvimento. Veja o [roadmap](#roadmap) para o que já está pronto.

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Front-end | Vue.js 3 (Composition API), Vue Router, Axios |
| Back-end | C# com ASP.NET Core Web API (.NET 10) |
| Acesso a dados | Entity Framework Core |
| Banco de dados | PostgreSQL |
| Versionamento | Git e GitHub (branches por funcionalidade e Pull Requests) |

## Estrutura do repositório

```
painel-produtos/
├── backend/
│   └── ProdutosApi/      # API em C# (Controllers, Models, Data)
├── frontend/             # Aplicação Vue.js (em breve)
├── database/             # Scripts SQL (em breve)
└── README.md
```

## Modelo de dados

- **Categorias**: `Id`, `Nome`
- **Produtos**: `Id`, `Nome`, `Descricao`, `Preco`, `Estoque`, `CategoriaId`, `Ativo`, `DataCriacao`, `DataAtualizacao`
- **HistoricoProdutos**: `Id`, `ProdutoId`, `CampoAlterado`, `ValorAntigo`, `ValorNovo`, `DataAlteracao`

Relacionamentos: uma categoria possui muitos produtos; um produto possui muitos registros de histórico.

## Como executar

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS), para o front-end
- [Docker](https://www.docker.com/), para o PostgreSQL
- Git

### 1. Clonar o repositório

```bash
git clone https://github.com/VictorHGomes/painel-produtos.git
cd painel-produtos
```

### 2. Subir o banco de dados

```bash
docker run --name pg-produtos -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=produtos -p 5432:5432 -d postgres
```

### 3. Configurar a conexão

Em `backend/ProdutosApi/appsettings.json`, a string de conexão padrão aponta para o banco local acima:

```json
"ConnectionStrings": {
  "Default": "Host=localhost;Port=5434;Database=produtos;Username=postgres;Password=postgres"
}
```

> As credenciais acima são apenas para desenvolvimento local. Em qualquer outro ambiente, use variáveis de ambiente ou `dotnet user-secrets`.

### 4. Criar as tabelas

```bash
cd backend/ProdutosApi
dotnet tool install --global dotnet-ef
dotnet ef database update
```

### 5. Executar a API

```bash
dotnet run
```

A API ficará disponível no endereço exibido no terminal (por exemplo, `http://localhost:5221`).

## Roadmap

- [x] Estrutura do repositório e `.gitignore`
- [x] Projeto ASP.NET Core Web API
- [x] Entidades `Categoria`, `Produto` e `HistoricoProduto`
- [x] `AppDbContext` (Entity Framework Core)
- [x] Migration inicial e criação das tabelas
- [ ] Scripts SQL (criação, carga inicial e ajuste em massa)
- [ ] Endpoints de produtos (listagem com filtro e paginação, detalhe, criação, alteração, ativar/desativar)
- [ ] Endpoint de categorias
- [ ] Histórico de alterações
- [ ] Validações e tratamento global de erros
- [ ] Camadas Controller, Service e Repository com DTOs
- [ ] Testes unitários (xUnit)
- [ ] Telas em Vue.js (listagem, cadastro/edição, detalhe com histórico)
- [ ] Documentação da API (Swagger/OpenAPI)
- [ ] Prints das telas neste README

### Ideias futuras

- Autenticação com JWT
- Docker Compose para subir tudo com um comando
- Importação de produtos via CSV
- Pipeline de CI com GitHub Actions

## Fluxo de trabalho

Cada funcionalidade é desenvolvida em uma branch própria (`feature/...`) e integrada à `main` por Pull Request. Os commits seguem o padrão Conventional Commits (`feat:`, `fix:`, `chore:`, `docs:`, `test:`).

## Autor

**Victor Hugo Gomes**
[LinkedIn](https://linkedin.com/in/victorgomesdev/) | [GitHub](https://github.com/VictorHGomes)
