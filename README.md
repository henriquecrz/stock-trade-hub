# stock-trade-hub

## O que foi feito

Desenvolvimento de uma API escrita em C# (.NET) que faz o controle de ativos e transações de compra e venda.

## Estratégia de Idempotência

Criação de uma lista em memória que salva todas as requisições de transações, assim, antes de realizar efetivamente a transação, verifica-se através do identificador único e status se a requisição em questão já foi processada ou não.

## Rotas

Para mais detalhes consulte o Swagger disponível em https://localhost:44365/swagger/index.html.

### Ativos

- POST /Stock/Create: Cria um novo ativo
  - A propriedade "code" deve possuir pelo menos 1 caracter
  - A propriedade "amount" deve ser maior ou igual a 0 (zero)
  - A propriedade "price" deve ser maior que 0 (zero)

```json
{
  "code": string,
  "amount": int,
  "price": decimal
}
```

- GET /Stock/Get: Retorna todos os ativos

- GET /Stock/GetByCode: Retorna o ativo a partir de um determinado código
  - Deve informar o parâmetro "code", que é o identificador único do ativo

- PUT /Stock/Update: Atualiza os dados de um determinado ativo
  - Deve informar o parâmetro "code", que é o identificador único do ativo
  - A propriedade "code" deve possuir pelo menos 1 caracter
  - A propriedade "amount" deve ser maior ou igual a 0 (zero)
  - A propriedade "price" deve ser maior que 0 (zero)

```json
{
  "code": string,
  "amount": int,
  "price": decimal
}
```

- DELETE /Stock/Delete: Deleta o ativo a partir de um determinado código
  - Deve informar o parâmetro "code", que é o identificador único do ativo

### Carteira



## Tecnologias

- C#
- .NET 7
- LINQ
- xUnit.net
  - A razão da escolha é porque na entrevista comentaram que utilizavam :)
- Moq
- RabbitMQ
- Swagger
- HostedService
  - Serviço consumidor da fila RabbitMQ

## Instruções

- Instalar Docker na máquina: https://www.docker.com/products/docker-desktop
- Baixar a imagem do MySQL do Docker Hub: docker pull mysql
- Executar a instância de banco de dados MySQL: docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=a -e MYSQL_DATABASE=cashflow -p 3306:3306 -d mysql
- No path raiz executar o comando: EntityFrameworkCore\Update-Database
- Navegar até o path cash-flow/api/ e executar o comando: dotnet build
- No mesmo path executar: dotnet run
- Acessar Swagger: http://localhost:5129/swagger/index.html
- Para rodar os testes: dotnet test
