using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.models
{
    public class ServerModel
    {
        private List<SocketModel> clients = new List<SocketModel>();
        enum Roles
        {
            Admin,
            Glack
        }

        public SocketModel server = new SocketModel
        {
            Name = "Server",
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
            token = new CancellationTokenSource(),
            role = 0
        };


        
        public ServerModel()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            server.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.socket.Bind(ipPoint);
            server.socket.Listen(10);
            clients.Add(server);
            ListenClients();
        }

        public async Task ListenClients()
        {
            while (!server.token.IsCancellationRequested)
            {
                var client = await server.socket.AcceptAsync();

                clients.Add(new SocketModel
                {
                    Name = null,
                    socket = client,
                    token = new CancellationTokenSource(),
                    role = 1
                });

                ReciveMessage(clients.Last());
            }
        }

        public async Task ReciveMessage(SocketModel client)
        {
            while (!client.token.IsCancellationRequested)
            {
                byte[] bytes = new byte[1024];
                await client.socket.ReceiveAsync(bytes, SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                string validString = ServermModel.ValidMessage(client.socket, clients, server, message);
                Logs.write(validString);
                if (!await Comm(message, client.socket))
                {
                    SendAllUser(validString, message);
                }
            }
        }


        private async Task SendMessage(SocketModel client, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            client.socket.SendAsync(bytes, SocketFlags.None);

        }

        private async Task<bool> Comm(string message, Socket client)
        {
            var socket = clients.First(element => element.socket == client);
            if (message.Contains("/username"))
            {
                string userName = message.Remove(0, 9);
                
                if (clients.Count == 2)
                {
                    clients[1].role = 0;
                }

                if (socket.Name == null)
                {
                    await SendAllUser(ServermModel.ValidMessage(socket.socket, clients, server, $"{userName} - подключился", true), "gg");
                }
                await SendMessage(socket, ServermModel.ValidMessage(socket.socket, clients, server, message));
                socket.Name = userName;
                await SendMessage(socket, ServermModel.ValidMessage(socket.socket, clients, server, "Никнейм был успешно изменен!", true));
            }

            else if (message.Contains("/exit"))
            {

                if (socket.role == (int)Roles.Admin)
                {
                    foreach (var val in clients)
                    {
                        if (clients.IndexOf(val) != 0)
                        {
                            await SendMessage(val, "$/exit$");
                            val.socket.Close();
                        }
                        
                    }

                    clients[0].socket.Close();
                    clients[0].socket.Dispose();
                    Logs.clear();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                            clients.Remove(clients[i]);
                    }
                }
                else
                {
                    await SendMessage(socket, "$/exit$");
                    socket.token.Cancel();
                    socket.token.Dispose();

                    for (int i = 0; i < clients.Count(); i++)
                    {
                        if (socket.Name == clients[i].Name)
                        {
                            clients.Remove(clients[i]);
                        }
                    }

                    await SendAllUser(ServermModel.ValidMessage(socket.socket, clients, server, $"Пользователь {socket.Name} вышел", true), "gg");
                }
            }

            else if (message.Contains("/allUser"))
            {
                await SendMessage(socket, message);
                foreach (var element in clients)
                {
                    await SendMessage(socket, ServermModel.ValidMessage(socket.socket, clients, server, element.Name, true));
                }
            }

            else if (message.Contains("/logs") && socket.role == (int)Roles.Admin)
            {
                await SendMessage(socket, message);
                foreach (var element in Logs.read())
                {
                    await SendMessage(socket, element);
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public async Task SendAllUser(string message, string noValid)
        {
            if (!await Comm(noValid, server.socket))
            {
                foreach (var element in clients)
                {
                    await SendMessage(element, message);
                }
            }

        }
    }
}
