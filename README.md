# Rabbitmq + Easynetq Communication for Microservices
Communication over RabbitMQ instance using EasynetQ for .NET and other platform's AMQP protocol implementations.

## Introduction to Problem area
[EasyNetQ](https://github.com/EasyNetQ/EasyNetQ "Read more about EasyNetQ") is a very simple RabbitMQ client API avaialble for .NET. This library simplicity is achieved through their own naming convention. 
Which means all the queue, exchange names, bindings, etc. all are handled by the library. When comes to .NET microservice this library is very useful when all the .NET projects are shared common
library for EventBus message formats. But when comes to microservices projects consume multiple backend prgramming languages, such as .NET Core 3 + Node.js or .NET Core 3 + .NET Framework MVC, 
I've come accoross that this default naming convention became not usable for me due to below reasons.

1. Queue naming covention - Since easynetq provide it's own queue name based the message "Class name, Namespace", it's hard to specify exact queue name when 
communicate with another AMQP protocol library. Ex: .NET Core + EasyNetQ -> Node.JS + [amqplib](https://github.com/squaremo/amqp.node "Read more about amqplib").
2. Serialization & Deserialization - EasynetQ use fully qualified assembly name during serialization and deserialization of messages. This is problematic when sending messages from
another AMQP protocol library to EasyNeqQ library. Ex: Node + Amqplib to .NET + EasynetQ.


This repository address solution to above concerned problem areas.
