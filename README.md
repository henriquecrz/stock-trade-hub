# stock-trade-hub

## O que foi feito

Desenvolvimento de uma API escrita em C# (.NET) que faz o controle de ativos e transações de compra e venda.

## Estratégia de Idempotência

Criação de uma lista em memória que salva todas as requisições de transações, assim, antes de realizar efetivamente a transação, verifica-se através do identificador único e do status se a requisição em questão já foi processada ou não.

## Rotas

Para mais detalhes execute a aplicação e consulte o Swagger.

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
  - Deve informar o parâmetro "code" de um ativo existente

- PUT /Stock/Update: Atualiza os dados de um determinado ativo
  - Deve informar o parâmetro "code" de um ativo existente
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
  - Deve informar o parâmetro "code" de um ativo existente

### Carteira

- POST /Wallet/Transact: Realiza uma transação de compra ou venda de um tipo de ativo
  - A propriedade "code" deve ser de um ativo existente
  - A propriedade "amount" deve ser maior que 0 (zero)
  - A propriedade "type" deve ser 0 para Comprar ou 1 para Vender

```json
{
  "stock": {
    "code": string,
    "amount": int
  },
  "type": int
}
```

- GET /Wallet/GetWallet: Retorna um resumo da carteira

- GET /Wallet/GetTransactions: Retorna todas as transações realizadas

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
- GitHub Actions
  - Sempre que a branch "main" recebe algum código novo o CI é acionado, executando o build da aplicação e os testes

## Instruções

- Instalar Docker na máquina: <https://www.docker.com/products/docker-desktop>
- Baixar a imagem do RabbitMQ do Docker Hub: ```docker pull rabbitmq:latest```
- Executar a instância do RabbitMQ: ```docker run -d --hostname rabbitserver --name rabbitmq-server -p 15672:15672 -p 5672:5672 rabbitmq:3-management```
- Navegar até o path stock-trade-hub/api/ e executar os comandos:
  - ```dotnet restore```
  - ```dotnet build```
  - ```dotnet run```
- Acessar Swagger: <http://localhost:5041/swagger/index.html>
- Para rodar os testes: ```dotnet test```
