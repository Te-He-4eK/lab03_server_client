using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static Dictionary <int, string> nameIdPairs = new Dictionary <int, string> ();

    static void Main()
    {
        TcpListener listener = new TcpListener(IPAddress.Loopback, 8888);
        listener.Start();
        Console.WriteLine("Сервер запущен!");
        LoadId();

        while (true)
        {
            using (TcpClient client = listener.AcceptTcpClient())
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string request = reader.ReadLine();
                Console.WriteLine("Запрос сервера: " + request);

                string[] parts = request.Split(' ');
                string action = parts[0];

                switch (action)
                {
                    case "GET":
                        string identifier = (parts[1] == "BY_NAME") ? parts[2] : GetById(parts[2]);

                        if (File.Exists($"D:\\vsvs\\lab3_server\\lab3_server\\bin\\Debug\\net6.0\\data\\{identifier}"))
                        {
                            writer.WriteLine($"Файл загружен! Укажите название:");
                            string saveFileName = Console.ReadLine();
                            File.Copy($"D:\\vsvs\\lab3_server\\lab3_server\\bin\\Debug\\net6.0\\data\\{identifier}", $"D:\\vsvs\\lab3_server\\lab3_server\\bin\\Debug\\net6.0\\data\\{saveFileName}");
                            writer.WriteLine($"Файл сохранен на диск!");
                        }
                        else
                        {
                            writer.WriteLine("404");
                        }
                        break;
                    case "PUT":
                        string content = parts[2];
                        string newFileName = $"D:\\vsvs\\lab3_server\\lab3_server\\bin\\Debug\\net6.0\\data\\{parts[1]}";
                        if (!File.Exists(newFileName))
                        {
                            string directoryPath = Path.GetDirectoryName(newFileName);
                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }
                            File.WriteAllText(newFileName, content);
                            nameIdPairs[nameIdPairs.Count + 1] = parts[1];
                            writer.WriteLine("200");
                        }
                        else
                        {
                            writer.WriteLine("403");
                        }
                        break;
                    case "DELETE":
                        string name = (parts[1] == "BY_NAME") ? parts[2] : GetById(parts[2]);
                        if (File.Exists($"D:\\vsvs\\lab3_server\\lab3_server\\bin\\Debug\\net6.0\\data\\{name}"))
                        {
                            File.Delete($"D:\\vsvs\\lab3_server\\lab3_server\\bin\\Debug\\net6.0\\data\\{name}");
                            DeleteByName(name);
                            writer.WriteLine("200");
                        }
                        else
                        {
                            writer.WriteLine("404");
                        }
                        break;
                    case "EXIT":
                        SaveId();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    static async void LoadId()
    {
        using (StreamReader r = new StreamReader("C:\\Users\\Руслан\\OneDrive\\Рабочий стол\\ids.txt"))
        {
            string? line;
            while ((line = await r.ReadLineAsync()) != null)
            {
                nameIdPairs[int.Parse(line.Split()[0])] = line.Split()[1];
            }
        }
    }

    static void SaveId()
    {
        using (StreamWriter w = new StreamWriter("C:\\Users\\Руслан\\OneDrive\\Рабочий стол\\ids.txt"))
        {
            foreach (int key in nameIdPairs.Keys)
            {
                w.WriteLine($"{key} {nameIdPairs[key]}");
            }
        }
    }

    static string GetById(string id)
    {
        return nameIdPairs[int.Parse(id)];
    }

    static void DeleteByName(string name)
    {
        foreach (int key in nameIdPairs.Keys)
        {
            if (nameIdPairs[key] == name)
            {
                nameIdPairs.Remove(key);
                break;
            }
        }
    }

}
