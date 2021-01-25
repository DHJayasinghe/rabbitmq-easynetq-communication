'use strict';

const amqp = require("amqplib");

const opt = { credentials: amqp.credentials.plain('rabbitmq_adm1n', 'Admin@#789') };
const RABBITMQ = "amqp://localhost:5672/CUSTOM_VHOST";

const open = amqp.connect(RABBITMQ, opt);
const q = "test.communication.queue";
const exchange = "mycustom.exchange";

let now = () => {
    let date_ob = new Date(); // current date

    // adjust 0 before single digit date
    let date = ("0" + date_ob.getDate()).slice(-2);
    let month = ("0" + (date_ob.getMonth() + 1)).slice(-2); // current month
    let year = date_ob.getFullYear(); // current year
    let hours = date_ob.getHours(); // current hours
    let minutes = date_ob.getMinutes();  // current minutes
    let seconds = date_ob.getSeconds(); // current seconds

    // prints date & time in YYYY-MM-DD HH:MM:SS format
    return `${year}-${month}-${date} ${hours}:${minutes}:${seconds}`;
};

// Consumer
open
    .then(function (conn) {
        console.log(`[ ${new Date()} ] Node.JS Send/Receive pattern rabbitmq send started`);
        return conn.createChannel();
    })
    .then(function (ch) {
        return ch.assertQueue(q, { durable: true })
            .then(async function (ok) {
                await ch.bindQueue(q, exchange, q);

                for (var i = 0; i < 50; i++) {
                    let remainder = i % 5;
                    if (remainder < 3 && i % 10 != 0) // intentionally skip multiplication of 10
                    {
                        var message = {
                            Name: `Order #: ${i}`,
                            DateTimeCreated: now(),
                            OrderItems: [{
                                Id: i,
                                ItemQty: i * 5,
                                Total: i * 5 * 780.5
                            }]
                        };
                        setTimeout(function (message) {
                            console.log(`Sending message: ${message.Name}`);
                            ch.sendToQueue(q,
                                Buffer.from(JSON.stringify(message)),
                                {
                                    type: "EventBusMessages.PlaceOrderRequestMessage", // explicitly specifed assembly name (use as a shared contract)
                                },
                            );
                        }, 1000 * remainder, message); // intentionally slow the execution
                    }
                }
            });
    })
    .catch(console.warn);