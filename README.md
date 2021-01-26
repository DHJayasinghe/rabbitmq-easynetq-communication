# Rabbitmq + Easynetq Communication for Microservices
Communication over RabbitMQ instance using EasynetQ for .NET and other platform's AMQP protocol implementations.

## About
[EasyNetQ](https://github.com/EasyNetQ/EasyNetQ "Read more about EasyNetQ") is a very simple RabbitMQ client API avaialble for .NET focusing on the simlicity. This library simplicity is achieved through their own naming convention. Which means all the queue, exchange names, bindings, etc. all are handled by the library. When comes to .NET microservices this library is very simple to use, when all the .NET projects are shared common library for EventBus message formats. But when comes to different projects with not shared common message format library or communicating with different AMQP libraries, such as .NET Core 3 to Node.js or .NET Core 3 to .NET Framework MVC, I've come accoross that this default convention is not usable considering below reasons.

1. Queue naming covention - Since easynetq provide it's own queue name based the message "Class name, Namespace", it's hard to specify exact queue name when 
communicate with another AMQP protocol library. Ex: .NET Core + EasyNetQ -> Node.JS + [amqplib](https://github.com/squaremo/amqp.node "Read more about amqplib").
2. Serialization & Deserialization - EasynetQ use fully qualified assembly name during serialization and deserialization of messages. This is problematic when sending messages from
another AMQP protocol library to EasyNeqQ library. Ex: Node + Amqplib to .NET + EasynetQ.


This repository mainly for the usage of EasyNetQ libraries concerning given considerations. This repository covers below communication patterns of EasyNetQ library.
1. Send/Receive pattern
2. RPC (Remote Procedure Call) pattern


## How to Build
1. Run `docker-compose up -d --buil` to run the RabbitMQ instance configured with 2 queues as `test.communication.queue` - for Send/Receive example and `payment.validity.check.queue` for RPC example.
2. Build Node.js project using `npm install` command on the root of the project folder named `Node`

## Examples
1. Send/Receive style projects - You can find configuration file named `EventBusTypeNameSerializer.cs` file which override default full qualified assembly name convention. Examples to try:
    1. .NET Core to .NET Core - Run `DotnetCore.Receiver` and `DotnetCore.Sender` projects
    2. .NET Core to Node.Js -  Run `NodeJS/sendreceive_receiver.js` and `DotnetCore.Sender` projects
    3. Node.Js to .NET Core - Run `DotnetCore.Receiver` and `NodeJS/sendreceive_sender.js` projects
2. RPC style projects - You can find configuration file named `EventBusTypeNameSerializer.cs` file which override default full qualified assembly name convention and `EventBusConventions.cs` file which use the `QueueAttribute` queue name instead of default queue name convention.
    1. .NET Core to .NET Core - Run `DotnetCore.Rpc.Responder` and `DotnetCore.Rpc.Requester` projects
    2. .NET Core to Node.Js - Run `NodeJS/rpc_responder.js` and `DotnetCore.Rpc.Requester` projects
    
## Things TODO:
1. .NET EasyNetQ to RabbitMQ Java Client communication
2. .NET EasyNetQ to RabbitMQ Python Client communication
