using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Chat_Client;
using static System.Net.Mime.MediaTypeNames;

IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 8888);
Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Bind(iPEndPoint);
socket.Listen();

//Создание списка клиентов
List<Socket> sockets = new List<Socket>();

Dictionary<string, string> people = new Dictionary<string, string>();

Console.WriteLine(JsonSerializer.Serialize(new Msg_server_c("list_client", "", people)));


Console.WriteLine("Сервер развернут по адресу:" + socket.LocalEndPoint);
Console.WriteLine("Ожидаем подключений");


while (true)
{

    Socket client = await socket.AcceptAsync();

    //Добавление нового клиента в список
    sockets.Add(client);

    task_server(client);
}

async Task task_server(Socket client)
{
    string name_client = "";
    string RemoteEP = client.RemoteEndPoint.ToString();

    try
    {
        while (true)
        {

            var buffer = new List<byte>();  //буффер для накопления входящих данных
            var bytesRead = new byte[1];
            //byte[] buffer = new byte[512];
            while (true)
            {
                var count = await client.ReceiveAsync(bytesRead, SocketFlags.None);
                //если считанный байт представляет конечный символ то выходим
                if (count == 0 || bytesRead[0] == '\n') break;
                //иначе добавляем в буффер
                buffer.Add(bytesRead[0]);
            }

            string responseText = Encoding.UTF8.GetString(buffer.ToArray());

            Console.WriteLine("responseText" + responseText);
            // int bytes = 0; // количество считанных байтов
            // считываем данные 


            //bytes = await client.ReceiveAsync(buffer, SocketFlags.None);
            // добавляем полученные байты в список

            // выводим отправленные клиентом данные
            //var responseText = Encoding.UTF8.GetString(buffer, 0, bytes);

            string resp_json = responseText.ToString();

            Msg_server_c? r_m_t = JsonSerializer.Deserialize<Msg_server_c>(resp_json);

            Console.WriteLine($"r_m_t.Command= {r_m_t.Command}-- r_m_t.Text {r_m_t.Text}");


            if (r_m_t.Command.IndexOf("name") == 0)
            {
                name_client = r_m_t.Text;

                byte[] requestData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Msg_server("add_client", name_client, "server", null)) + '\n');

                if (!people.ContainsKey(RemoteEP))
                {
                    people.Add(RemoteEP, name_client);
                }
                else
                {
                    people[RemoteEP] = name_client;
                }

                await client.SendAsync(requestData, SocketFlags.None);

                Console.WriteLine("команда-имя");
            }
            else if (r_m_t.Command.IndexOf("get_clients") == 0)
            {
                /* string all_name_cln = "";
                 foreach (var person in people)
                 {
                     all_name_cln += $"key: {person.Key}  value: {person.Value}\n";
                 }*/

                byte[] requestData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Msg_server("list_client", "", name_client, people)) + '\n');

                await client.SendAsync(requestData, SocketFlags.None);

                Console.WriteLine("команда-пользователи");
            }

            else if (r_m_t.Command.IndexOf("file") == 0)
            {
                //string path = @"C:\Users\user\Pictures\VBq-mzZR5aM.jpg";   // путь к файлу

                string path = $"{r_m_t.Text}";
                // чтение из файла
                using (FileStream fstream = File.OpenRead(path))
                {

                    byte[] requestData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Msg_server("file", fstream.Length.ToString(), "server", null)) + '\n');

                    await client.SendAsync(requestData, SocketFlags.None);

                    byte[] buffer_file = new byte[fstream.Length];
                    await fstream.ReadAsync(buffer_file, 0, buffer_file.Length);

                    await client.SendAsync(buffer_file, SocketFlags.None);
                }
            }

            else if (r_m_t.Command.IndexOf("msg") == 0)
            {

                byte[] requestData = Encoding.UTF8.GetBytes(resp_json + '\n');

                foreach (Socket socket in sockets)
                {
                    //await socket.SendAsync(requestData, SocketFlags.None);

                    if (socket != client)
                    {
                        await socket.SendAsync(requestData, SocketFlags.None);
                    }
                    else
                    {
                        r_m_t.Command = "my_msg";
                        byte[] requestData2 = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(r_m_t) + '\n');
                        await socket.SendAsync(requestData2, SocketFlags.None);
                    }

                }

                Console.WriteLine(resp_json);
            }

            Console.WriteLine($"Новый клиент - {name_client}  remoteEP - {RemoteEP}");
        }
    }

    catch (SocketException)
    {
        sockets.Remove(client);
        people.Remove(client.RemoteEndPoint.ToString());
        Console.WriteLine($"Не удалось установить подключение с {socket.RemoteEndPoint} - {name_client}");
    }
    catch (IOException ex)
    {
        Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при передаче файла: {ex.Message}");
    }

}


