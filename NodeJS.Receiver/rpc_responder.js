'use strict';

const amqp = require("amqplib");

const opt = { credentials: amqp.credentials.plain('rabbitmq_adm1n', 'Admin@#789') };
const RABBITMQ = "amqp://localhost:5672/CUSTOM_VHOST";

const open = amqp.connect(RABBITMQ, opt);
const q = "test.communication.queue";
const exchange = "mycustom.exchange";

// Consumer
open
    .then(function (conn) {
        console.log(`[ ${new Date()} ] Node.JS Send/Receive pattern rabbitmq receiver started`);
        return conn.createChannel();
    })
    .then(function (ch) {
        return ch.assertQueue(q, { durable: true })
            .then(async function (ok) {
                await ch.bindQueue(q, exchange, q);
                return ch.consume(q, async function (msg) {
                    const message = JSON.parse(msg.content.toString("utf8"));
                    console.log(message);
                    console.log(`[ ${new Date()} ] Message received: ${message}`);

                    // acknowledge as message received
                    ch.ack(msg);
                });
            });
    })
    .catch(console.warn);