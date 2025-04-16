# ATM system
Implementation of ATM system

# Functional requirements

- account creation
- account balance view
- account withdrawal
- account replenishment
- transaction history view
# Non-functional requirements

- interactive console interface
- ability to select the operating mode (user, administrator)
- when selecting a user, account data (number, PIN) must be requested
- when selecting an administrator, a system password must be requested
- if the password is entered incorrectly, the system stops working
- the system password must be parameterizable
- when attempting to perform incorrect operations, error messages must be displayed
- data must be persistently stored in the database (PostgreSQL)
- using any ORM libraries is prohibited
- the application must have a hexagonal architecture
  - optional: you can implement an onion architecture with a rich domain model.
# Test cases

- withdrawing money from an account
- if there is a sufficient balance, check that the account is saved with a correctly updated balance
- if there is an insufficient balance, the service should return an error
- replenishing an account
- check that the account is saved with a correctly updated balance \
These tests check the business logic, they do not depend on the database or console
representation. \
The tests use mock repositories.
---
