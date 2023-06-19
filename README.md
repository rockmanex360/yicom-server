# yicom-server

The application is built using .net core 6 webapp. Run as a SignalR host and a listener for RabbitMq
incoming queue using simple direct exchange. Once the application received the message, It will filter the message only for Priority >= 7 and forwarded it to Front-End through SignalR socket.

## How to install

1. Run a RabbitMq server on docker. you can use this commnand
`docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.12-management`

2. Once done, run restore command on yicom-server.
`dotnet restore`

3. Build the application and run.
`dotnet build` & `dotnet run`