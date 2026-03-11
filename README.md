# 🏦 Teste Técnico — Banco do Paraná

Sistema de crédito baseado em microserviços com comunicação assíncrona via RabbitMQ, desenvolvido em .NET 8 com arquitetura DDD.

---

## 📋 Checklist do Desafio

| Requisito | Status |
|---|---|
| Cadastro de clientes via API | ✅ |
| Geração de proposta de crédito | ✅ |
| Emissão de até 2 cartões de crédito | ✅ |
| Comunicação via mensageria (RabbitMQ) | ✅ |
| Resiliência no processamento de mensagens | ✅ |
| Testes unitários com xUnit | ✅ |
| Fluxograma da solução | ✅ |
| Docker para todos os serviços | ✅ |
| API documentada com Swagger | ✅ |
| Internacionalização (pt / en / es) | ✅ |

---

## 📊 Fluxograma da Solução

![Fluxograma](./docs/FluxogramaTestesTecnico_drawio.png)

O fluxo completo funciona da seguinte forma:

1. O cliente realiza um `POST /api/clientes` no **ClienteService**
2. O ClienteService persiste no `ClienteDB` e publica o evento `NovoCliente` no RabbitMQ
3. O **PropostaService** consome o evento, analisa o score, gera a proposta e salva no `PropostaDB`
4. O PropostaService publica o evento `NovaProposta` no RabbitMQ
5. O **CartaoService** consome o evento e emite 1 ou 2 cartões, salvando no `CartaoDB`

---

## 🏗️ Arquitetura

```
POST /api/clientes
        │
        │ client-created-integration-event
        ▼
  ┌─────────────────┐
  │    RabbitMQ     │
  └────────┬────────┘
           │
           ▼
  [PropostaService] → analisa score → salva proposta
           │
           │ proposta-aprovada-integration-event
           ▼
  ┌─────────────────┐
  │    RabbitMQ     │
  └────────┬────────┘
           │
           ▼
  [CartaoService] → emite 1 ou 2 cartões
```

### Regras de Score

| Score | Resultado | Limite por Cartão | Cartões |
|---|---|---|---|
| 0 — 100 | ❌ Recusado | R$ 0 | 0 |
| 101 — 500 | ✅ Aprovado | R$ 1.000 | 1 |
| 501 — 1000 | ✅ Aprovado | R$ 5.000 | 2 |

---

## 🗂️ Estrutura do Projeto

```
solution/
├── src/
│   ├── ClienteService/
│   │   ├── ClienteService.Api/            ← Web API (POST /api/clientes)
│   │   ├── ClienteService.Application/    ← Handlers, Commands, DTOs
│   │   ├── ClienteService.Domain/         ← Entidades, Value Objects, Eventos
│   │   └── ClienteService.Infrastructure/ ← EF Core, RabbitMQ, Repositórios
│   │
│   ├── PropostaService/
│   │   ├── PropostaService.Worker/        ← Worker Service (consome fila)
│   │   ├── PropostaService.Application/   ← Handlers, regras de análise
│   │   ├── PropostaService.Domain/        ← Entidade Proposta, Score
│   │   └── PropostaService.Infrastructure/← EF Core, RabbitMQ
│   │
│   ├── CartaoService/
│   │   ├── CartaoService.Worker/          ← Worker Service (consome fila)
│   │   ├── CartaoService.Application/     ← Handler de emissão
│   │   ├── CartaoService.Domain/          ← Entidade Cartão, Luhn, CVV
│   │   └── CartaoService.Infrastructure/  ← EF Core, RabbitMQ
│   │
│   └── Shared/
│       └── Shared.Integration/            ← Contratos de Integration Events
│
├── tests/
│   ├── ClienteService.Tests/              ← xUnit
│   ├── PropostaService.Tests/             ← xUnit
│   └── CartaoService.Tests/               ← xUnit
│
├── docker-compose.yml
└── README.md
```

---

## 🧱 Decisões Técnicas

### DDD (Domain-Driven Design)
Cada microserviço segue arquitetura em 4 camadas. O Domain é o núcleo sem dependências externas — as dependências sempre apontam para dentro (API → Application → Domain ← Infrastructure).

### MassTransit + RabbitMQ
Abstração sobre o RabbitMQ para roteamento de mensagens, serialização automática e reconexão. Cada serviço registra seus consumers via `ConfigureEndpoints`.

### Resiliência com Retry Exponencial
Configurado retry com backoff exponencial no MassTransit. Em caso de falha, o sistema tenta novamente até 5 vezes com intervalos crescentes, garantindo que mensagens não sejam perdidas em instabilidades temporárias de rede ou banco:

```csharp
cfg.UseMessageRetry(r =>
    r.Exponential(5,
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(30),
        TimeSpan.FromSeconds(5)));
```

### Internacionalização (i18n)
A API do ClienteService suporta múltiplos idiomas nas mensagens de resposta e validação. O idioma é detectado automaticamente pelo header `Accept-Language` da requisição:

| Idioma | Header |
|---|---|
| 🇧🇷 Português **(padrão)** | `Accept-Language: pt` ou ausente |
| 🇺🇸 Inglês | `Accept-Language: en` |
| 🇪🇸 Espanhol | `Accept-Language: es` |

### API Documentada com Swagger
O ClienteService possui documentação interativa via Swagger/OpenAPI com descrições dos endpoints, parâmetros, tipos de resposta e exemplos — permitindo testar a API diretamente pelo browser sem ferramentas externas.

Acesse: **http://localhost:5001**

### Algoritmo de Luhn
O número do cartão é gerado seguindo o padrão real da indústria financeira (ISO/IEC 7812), garantindo que qualquer número emitido passe na validação Luhn — o mesmo algoritmo usado por Visa e Mastercard.

### Domain Events + AppDbContext
Os eventos de domínio são disparados automaticamente após o `SaveChangesAsync`, garantindo que o banco seja persistido antes de qualquer evento ser publicado no RabbitMQ, evitando eventos órfãos de operações que falharam.

### Database per Service
Cada microserviço possui seu banco de dados isolado (`clientedb`, `propostadb`, `cartaodb`). Os serviços nunca acessam o banco um do outro — toda comunicação é exclusivamente via eventos de integração.

---

## 🚀 Como rodar localmente

### Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) *(apenas para desenvolvimento sem Docker)*

### Subindo o ambiente

```bash
# Clone o repositório
git clone https://github.com/2zedoj/Teste-Tecnico-Parana-Banco
cd seu-repositorio/solution

# Sobe todos os serviços
docker-compose up --build -d

# Acompanha os logs
docker-compose logs -f
```

> ⚠️ Aguarde ~40 segundos para o SQL Server inicializar antes de fazer requisições.

### Serviços disponíveis

| Serviço | Endereço |
|---|---|
| 📋 ClienteService API + Swagger | http://localhost:5001 |
| 🐰 RabbitMQ Management | http://localhost:15672 — `guest / guest` |
| 🗄️ SQL Server | `localhost,1439` — `sa / BancoParana@123` |

### Testando o fluxo completo

Acesse o Swagger em `http://localhost:5001` e envie um `POST /api/clientes`:

```json
{
  "name": "João Silva",
  "document": "123.456.789-09",
  "email": "joao.silva@email.com",
  "renda": 5000.00,
  "score": 700
}
```

**Resultado esperado por faixa de score:**

```
score 50   → proposta recusada, nenhum cartão emitido
score 200  → proposta aprovada, 1 cartão com limite R$ 1.000
score 700  → proposta aprovada, 2 cartões com limite R$ 5.000 cada
```

### Testando a internacionalização

```bash
# Mensagens de validação em inglês
curl -X POST http://localhost:5001/api/clientes \
  -H "Content-Type: application/json" \
  -H "Accept-Language: en" \
  -d '{"name": ""}'

# Mensagens de validação em espanhol  
curl -X POST http://localhost:5001/api/clientes \
  -H "Content-Type: application/json" \
  -H "Accept-Language: es" \
  -d '{"name": ""}'
```

### Derrubando o ambiente

```bash
docker-compose down          # Para e mantém os dados
docker-compose down -v       # Para e apaga os volumes (banco zerado)
```

---

## 🧪 Testes

```bash
# Todos os testes
dotnet test

# Por serviço
dotnet test tests/ClienteService.Tests/
dotnet test tests/PropostaService.Tests/
dotnet test tests/CartaoService.Tests/
```

### Cobertura

| Serviço | Casos testados |
|---|---|
| **ClienteService** | Criação com dados válidos, CPF inválido, CPF com máscara, documento vazio |
| **PropostaService** | Score baixo (≤100), médio (101-500), alto (501-1000), limites exatos das faixas (100 e 101) |
| **CartaoService** | Emissão de 1 cartão, 2 cartões, validação do algoritmo Luhn, formato do CVV |

---

## 📦 Tecnologias

| Tecnologia | Uso |
|---|---|
| .NET 8 | Runtime |
| ASP.NET Core Web API | ClienteService — entrada HTTP |
| Worker Service | PropostaService e CartaoService — consumers |
| MassTransit 8.2.3 | Abstração sobre RabbitMQ |
| RabbitMQ 3 | Message broker |
| Entity Framework Core 9 | ORM e migrations automáticas |
| SQL Server 2022 | Banco de dados |
| MediatR | CQRS e Domain Events |
| AutoMapper | Mapeamento de DTOs |
| xUnit | Testes unitários |
| Swagger / OpenAPI | Documentação interativa da API |
| Docker + Compose | Containerização |

---

## 📁 Bancos de Dados

| Banco | Serviço | Tabelas |
|---|---|---|
| `clientedb` | ClienteService | `Clients` |
| `propostadb` | PropostaService | `Propostas` |
| `cartaodb` | CartaoService | `Cartoes` |

> As migrations são aplicadas automaticamente na inicialização — nenhum comando manual necessário.

---

## 👤 Autor

Desenvolvido por **Gabriely Rodrigues** como parte do processo seletivo para a vaga de Desenvolvedor Pleno no **Banco do Paraná**.
