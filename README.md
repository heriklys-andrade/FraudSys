# 🔐 API de Gestão de Limites para Transações PIX

Este projeto é uma API desenvolvida em **.NET 8** com **DynamoDB (AWS)** para gerenciamento de **limites de transações PIX** por cliente. A aplicação permite cadastrar contas, gerenciar limites e validar transações conforme regras definidas.

---

## 📚 Funcionalidades

### 🧾 1. Cadastro de limite

Permite registrar uma conta bancária com um limite específico para transações PIX. Apenas uma conta pode ser criada por CPF.Cada conta é identificada por:

- CPF
- Número da agência
- Número da conta

📌 Todos os campos são obrigatórios.

---

### 🔍 2. Consulta de limite

Busca as informações de uma conta existente com base no CPF, agência e número da conta. Retorna:

- Limite atual disponível

---

### ✏️ 3. Atualização de limite

Atualiza o valor do limite PIX para uma conta previamente cadastrada.

---

### ❌ 4. Remoção de limite

Remove uma conta do banco de dados de limites, invalidando seu uso para futuras transações PIX.

---

### 💸 5. Validação de transações PIX

Recebe uma solicitação de transação e executa a lógica de validação do limite:

- ✅ Se o valor da transação for menor ou igual ao limite: **transação aprovada** e o valor é descontado do limite.
- ❌ Se o valor for superior ao limite: **transação negada**, e o limite permanece inalterado.

---

## ⚙️ Tecnologias Utilizadas

| Camada      | Tecnologias / Padrões                                                                              |
| ----------- | -------------------------------------------------------------------------------------------------- |
| Backend     | [.NET 8](https://dotnet.microsoft.com/)                                                            |
| Banco       | [Amazon DynamoDB (AWS - uso recomendado)](https://aws.amazon.com/dynamodb/)                        |
| Arquitetura | MVC, SOLID, Clean Code                                                                             |
| Extras      | [Domain Driven Design (DDD)](https://en.wikipedia.org/wiki/Domain-driven_design), Testes Unitários |

---

## ▶️ Como executar o projeto

### 🔐 Configuração do User Secrets (Recomendado)

Para armazenar com segurança suas credenciais da AWS em ambiente de desenvolvimento local, adicione o seguinte bloco no **User Secrets** da sua IDE (Visual Studio ou VS Code):

```json
"AWS": {
  "Region": "",
  "AccessKey": "",
  "SecretKey": ""
}
```

> 💡 Altere `AccessKey` e `SecretKey` com suas credenciais reais. O campo `Region` deve representar o código da região AWS (ex: `sa-east-1`, `us-east-1`, etc.).

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [AWS CLI configurado](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-quickstart.html)
- Conta AWS com permissões para usar DynamoDB (**recomendado**)  
  *(Opcional para testes locais: [DynamoDB Local](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.html))*

---

## 📡 Endpoints da API

| Método | URL                            | Entrada de dados | Descrição                                       |
| ------ | ------------------------------ | ---------------- | ----------------------------------------------- |
| POST   | `/v1/Clients`                  | Body             | Cadastrar nova conta e limite PIX (uma por CPF) |
| GET    | `/v1/Clients`                  | Query Parameters | Consultar limite por CPF, agência e conta       |
| DELETE | `/v1/Clients`                  | Query Parameters | Remover conta existente                         |
| PUT    | `/v1/Clients/{clientDocument}` | Path + Body      | Atualizar o limite de uma conta existente       |
| POST   | `/v1/Transactions/pix`         | Body             | Validar e registrar transação PIX               |

---

## 📄 Documentação da API

A API expõe uma documentação interativa via Swagger. Após iniciar o projeto, acesse:

[https://localhost:7179/swagger/index.html](https://localhost:7179/swagger/index.html)

---

## 🧠 Observações

- A aplicação está preparada para uso com **Amazon DynamoDB na AWS** (uso recomendado).
- Também pode ser executada com **DynamoDB Local** para desenvolvimento e testes offline.
- O middleware global de exceções padroniza os erros retornados pela API.
