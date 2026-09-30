# Painel de Manutenção de Produtos

Sistema web para cadastro, consulta, edição e desativação de produtos por meio de uma interface, com histórico de alterações. O objetivo é permitir que manutenções que antes exigiam chamadas diretas à API ou alterações manuais no banco sejam feitas por uma tela.

> **Status:** em desenvolvimento. Veja o [roadmap](#roadmap) para o que já está pronto.

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Front-end | Vue.js 3 (Composition API), Vue Router, Axios |
| Back-end | C# com ASP.NET Core Web API (.NET 10) |
| Acesso a dados | Entity Framework Core (Npgsql) |
| Banco de dados | PostgreSQL |
| Documentação da API | OpenAPI + Swagger UI |
| Versionamento | Git e GitHub (branches por funcionalidade e Pull Requests) |

## Estrutura do repositório

```
painel-produtos/
├── backend/
│   └── ProdutosApi/
│       ├── Controllers/  # Endpoints HTTP
│       ├── Services/     # Regras de negócio
│       ├── Dtos/         # Objetos de entrada e saída da API
│       ├── Models/       # Entidades do banco
│       ├── Data/         # AppDbContext e migrations
│       └── Program.cs
├── frontend/             # Aplicação Vue.js (em breve)
├── database/             # Scripts SQL (carga inicial e consultas)
└── README.md
```

## Modelo de dados

- **Categorias**: `Id`, `Nome`
- **Produtos**: `Id`, `Nome`, `Descricao`, `Preco`, `Estoque`, `CategoriaId`, `Ativo`, `DataCriacao`, `DataAtualizacao`
- **Historicos**: `Id`, `ProdutoId`, `CampoAlterado`, `ValorAntigo`, `ValorNovo`, `DataAlteracao`

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
docker run --name pg-produtos -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=produtos -p 5434:5432 -d postgres
```

> O container usa a porta **5434** no computador para não conflitar com um PostgreSQL já instalado, que normalmente ocupa a 5432.

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

### 5. Carregar os dados iniciais (opcional)

Insere 5 categorias e 30 produtos de exemplo. **Execute apenas uma vez**, pois rodar de novo duplica os dados.

```bash
cd ../..
docker cp database/02_dados_iniciais.sql pg-produtos:/tmp/02_dados_iniciais.sql
docker exec pg-produtos psql -U postgres -d produtos -f /tmp/02_dados_iniciais.sql
```

O arquivo `database/04_consultas.sql` reúne consultas de estudo e conferência (JOIN, GROUP BY e HAVING), somente leitura.

### 6. Executar a API

```bash
cd backend/ProdutosApi
dotnet run
```

A API ficará disponível no endereço exibido no terminal (por exemplo, `http://localhost:5221`).

## Documentação da API (Swagger)

Em ambiente de desenvolvimento, a interface do Swagger fica em `http://localhost:5221/swagger` (ajuste a porta conforme o terminal) e permite testar todos os endpoints pelo navegador. A especificação OpenAPI em JSON está em `/openapi/v1.json`.

### Endpoints disponíveis

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/produtos` | Lista produtos com filtro por `nome` e `categoriaId` e paginação (`pagina`, `tamanhoPagina`) |
| GET | `/api/produtos/{id}` | Detalhe de um produto (404 se não existir) |
| POST | `/api/produtos` | Cria um produto, com validação dos campos (201 ou 400) |
| GET | `/api/categorias` | Lista as categorias em ordem alfabética |

## Roadmap

- [x] Estrutura do repositório e `.gitignore`
- [x] Projeto ASP.NET Core Web API
- [x] Entidades `Categoria`, `Produto` e `HistoricoProduto`
- [x] `AppDbContext` (Entity Framework Core)
- [x] Migration inicial e criação das tabelas
- [x] Scripts SQL: carga inicial e consultas
- [ ] Scripts SQL: criação de tabelas e ajuste em massa de preços
- [x] Endpoints de produtos: listagem com filtro e paginação, detalhe e criação
- [ ] Endpoints de produtos: alteração e ativar/desativar
- [x] Endpoint de categorias
- [x] Documentação da API (Swagger/OpenAPI)
- [ ] Histórico de alterações
- [ ] Tratamento global de erros e CORS
- [ ] Camada Repository
- [ ] Testes unitários (xUnit)
- [ ] Telas em Vue.js (listagem, cadastro/edição, detalhe com histórico)
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