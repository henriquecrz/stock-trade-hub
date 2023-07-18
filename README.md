# stock-trade-hub

## O que foi feito

Desenvolvimento de uma API escrita em C# (.NET) que faz o controle de ativos e transações de compra e venda.

## Estrat�gia de Idempot�ncia

Criação de uma lista em memória que salva todas as requisições de transações, assim, antes de realizar efetivamente a transação, verifica-se através do identificador único e status se a requisição em questão já foi processada ou não.

## Rotas

- Ativos
  - POST /Stock/Create
    - ```json{
  "code": "string",
  "amount": 0,
  "price": 0
}```

- GET /Balance: retorna todos os lan�amentos criados em uma determinada data agrupados pelo tipo da moeda (ex: BRL, USD, etc)
  - Par�metros: date, formato ano-m�s-dia (ex: 2023-05-12)
- GET /CashFlow: retorna todos os lan�amentos criados
- POST /CashFlow: cria um lan�amento
  - Par�metros:
    - id: int
    - type: int (Enum)
      - Cr�dito: 0
      - D�bito: 1
    - currency: int (Enum)
      - BRL: 0
      - USD: 1
      - EUR: 2
    - value: double (decimal)
    - description: string
    - date: string (DateTime)

## Tecnologias

- C#
- .NET 7
- LINQ
- xUnit.net
  - A raz�o da escolha � porque na entrevista comentaram que utilizavam :)
- Moq
- RabbitMQ
- Swagger
- HostedService
  - Serviço consumidor da fila RabbitMQ

## Instru��es

- Instalar Docker na m�quina: https://www.docker.com/products/docker-desktop
- Baixar a imagem do MySQL do Docker Hub: docker pull mysql
- Executar a inst�ncia de banco de dados MySQL: docker run --name mysql-container -e MYSQL_ROOT_PASSWORD=a -e MYSQL_DATABASE=cashflow -p 3306:3306 -d mysql
- No path raiz executar o comando: EntityFrameworkCore\Update-Database
- Navegar at� o path cash-flow/api/ e executar o comando: dotnet build
- No mesmo path executar: dotnet run
- Acessar Swagger: http://localhost:5129/swagger/index.html
- Para rodar os testes: dotnet test
