'use strict';

const amqp = require("amqplib");

const opt = { credentials: amqp.credentials.plain('rabbitmq_adm1n', 'Admin@#789') };
const RABBITMQ = "amqp://localhost:5672/CUSTOM_VHOST";

const open = amqp.connect(RABBITMQ, opt);
const q = "payment.validity.check.queue"; // our custom queue name given on QueueAttribute
const rpc_exchange = "easy_net_q_rpc"; // easyneq use this default exchange for rpc style communication

// Consumer
open
    .then(function (conn) {
        console.log(`[ ${new Date()} ] Node.JS Send/Receive pattern rabbitmq receiver started`);
        return conn.createChannel();
    })
    .then(function (ch) {
        return ch.assertQueue(q, { durable: true })
            .then(async function (ok) {
                await ch.bindQueue(q, rpc_exchange, q);
                return ch.consume(q, async function (msg) {
                    const request = JSON.parse(msg.content.toString("utf8"));
                    console.log(request);
                    console.log(`[ ${new Date()} ] Received request: ${JSON.stringify(request)}`);

                    let valid = request.CustomerId > 0 && request.Amount > 0 && request.PaymentType === "CASH";
                    let message = {
                        Valid: valid,
                        Message: valid ? "Valid payment" : "Invalid payment"
                    };
                    ch.sendToQueue(
                        msg.properties.replyTo, // reply to callback queue name
                        Buffer.from(JSON.stringify(message)),
                        {
                            correlationId: msg.properties.correlationId,
                            type: "EventBusMessages.PaymentValidityCheckResponseMessage", // our qualified assembly name of the message format (shared contract)
                        },
                    );
                    // acknowledge as message received
                    ch.ack(msg);
                });
            });
    })
    .catch(console.warn);