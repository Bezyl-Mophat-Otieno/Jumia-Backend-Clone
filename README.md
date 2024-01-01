
# Jumia Backend Clone .

This Ecommerce application is designed using a microservices architecture comprising four individual services: AuthMS, ProductMS, TransactionMS, and CouponMS. Each service is responsible for specific functionalities within the application.

## Overview

The Ecommerce Microservices Application is a modular, scalable, and distributed system built to handle various aspects of an online retail platform. Each microservice operates independently, focusing on specific functionalities to ensure flexibility and maintainability.

## Services

### 1. AuthMS (Authentication Microservice)

Responsible for managing user authentication and authorization.

- **Features**:
  - User signup, login, and authentication.
  - Role-based access control.
  - JSON Web Token (JWT) generation and validation.

### 2. ProductMS (Product Microservice)

Handles all operations related to products available in the Ecommerce platform.

- **Features**:
  - Product management: creation, retrieval, update, deletion (CRUD operations).
  - Catalog management and search functionalities.
  - Product details, categories, and inventory management.

### 3. TransactionMS (Transaction Microservice)

Manages all transaction-related activities and order processing.

- **Features**:
  - Order creation, tracking, and fulfillment.
  - Transaction history and order status tracking.
  - Integration with payment gateways.

### 4. CouponMS (Coupon Microservice)

Deals with coupon creation, distribution, and validation for discounts.

- **Features**:
  - Coupon generation, distribution, and redemption.
  - Discount calculation and application during checkout.
  - Coupon validity and usage tracking.
  
Absolutely, integrating an Azure Service Bus for inter-service communication is crucial in a microservices architecture. Here's an enhanced README section that includes information about the JumiaAzureServiceBus class library:

---

## Interservice Communication

In addition to the individual microservices (AuthMS, ProductMS, TransactionMS, and CouponMS), the Ecommerce application employs an interservice communication mechanism facilitated by the `JumiaAzureServiceBus` class library. This library handles communication between microservices using Azure Service Bus queues and topics.

### JumiaAzureServiceBus

The `JumiaAzureServiceBus` class library is responsible for managing communication channels and message queues/stacks between microservices.

- **Features**:
  - **Queues and Topics**: Utilizes Azure Service Bus queues and topics for asynchronous messaging.
  - **Message Broker**: Enables decoupled communication between microservices.
  - **Message Serialization**: Handles serialization and deserialization of messages.

### Usage

1. **Setup Azure Service Bus**:
   - Configure Azure Service Bus namespaces, queues, and topics in your Azure portal.
   - Obtain connection strings for authentication.

2. **Configuration**:
   - Provide Azure Service Bus connection details in the configuration file or environment variables.

3. **JumiaAzureServiceBus Library**:
   - Use the library to send and receive messages between microservices.
   - Implement message handlers for specific actions or events triggered by messages.

## Technologies Used

-  ASP.NET 7 Core, Entity Framework Core, SQL Server, JWT Authentication, Docker, and Azure Service Bus.

## Usage

Here's the Link to the Documentation : https://documenter.getpostman.com/view/21431560/2s9YsDkueG

## License
MIT License .
