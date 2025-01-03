# TradeX

##  📋 Overview  

Scalable cryptocurrency trading simulation platform that facilitates the trading of cryptocurrencies. Users can buy, sell, and trade digital assets like Bitcoin, Ethereum, and other cryptocurrencies.

## Give a Star! ⭐
If you like this project, please give it a star. Thanks!

## 📄 Description

The system features a background job mechanism that dynamically updates the prices of various cryptocurrencies at random intervals.
These price fluctuations simulate the unpredictable nature of actual cryptocurrency markets.

Users can place two types of orders—Spot Orders and Future Orders—based on their preferred trading strategies:
- Spot Orders: Instantaneous buy or sell transactions at current market prices.
- Future Orders: Orders set to execute at a predetermined price point or condition, enabling users to strategize based on future market expectations.

Users can place buy or sell orders for cryptocurrencies based on their market analysis. 
The system automatically monitors the price changes and executes orders when the specified conditions are met, such as the price reaching a user-defined threshold.


## ⚙️ Technologies And Features
- .NET Core 9 / C# 13
-  Clean Architecture with Domain-Driven Design (DDD)
-  Entity Framework Core
-  CQRS with MediatR Pattern 
-  Unit of Work & Repository Pattern
-  JWT Authentication & Refresh Tokens
-  Password Hashing
-  Quartz for Background Job Processing
-  Fluent Validation
-  Manual Mapping
-  Serilog
-  Pagination
-  Outbox pattern for eventual Consistency
-  Result Pattern For Flow control
-  Unit Tests Using (XUnit, NSubstitute, Fluent Assertions, Bogus)
-  Integration Tests

## Postman Collection

The Postman collection for this project is available in the `postman` folder.

### How to Import
1. Open Postman.
2. Click **Import** in the top left.
3. Select the file.
4. Use the collection for testing the API.
