using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Client
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Выберите действие (1 - получить файл, 2 - создать файл, 3 - удалить файл, 4 - exit):");
            int action = int.Parse(Console.ReadLine());

            switch (action)
            {
                case 1:
                    GetFile();
                    break;
                case 2:
                    SaveFile();
                    break;
                case 3:
                    DeleteFile();
                    break;
                case 4:
                    Exit();
                    break;
                default:
                    Console.WriteLine("Ошибка. Попробуйте снова.");
                    break;
            }
        }
    }

    static void GetFile()
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 8888))
        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream))
        using (StreamWriter writer = new StreamWriter(stream))
        {
            Console.WriteLine("Какой вы хотите получить файл name или id (1 - name, 2 - id):");
            string method = Console.ReadLine() == "1" ? "BY_NAME" : "BY_ID";

            string fileName = string.Empty;
            if (method == "BY_NAME")
            {
                Console.WriteLine("Укажите имя для файла:");
                fileName = Console.ReadLine();
            } else
            {
                Console.WriteLine("Введите индентификатор");
                fileName = Console.ReadLine();
            }
            
            writer.WriteLine($"GET {method} {fileName}");
            writer.Flush();

            string response = reader.ReadLine();
            Console.WriteLine(response);


        }
    }

    static void SaveFile()
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 8888))
        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream))
        using (StreamWriter writer = new StreamWriter(stream))
        {
            Console.WriteLine("Введите имя файла, который будет сохранен на сервере:");
            string fileName = Console.ReadLine();

            Console.WriteLine("Введите содержимое: ");
            string fileContent = Console.ReadLine();

            writer.WriteLine($"PUT {fileName} {fileContent}");
            writer.Flush();

            string response = reader.ReadLine();
            Console.WriteLine(response);
        }
    }

    static void DeleteFile()
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 8888))
        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream))
        using (StreamWriter writer = new StreamWriter(stream))
        {
            Console.WriteLine("Какой файл вы хотите удалить name или id (1 - name, 2 - id):");
            string method = Console.ReadLine() == "1" ? "BY_NAME" : "BY_ID";

            string fileName = string.Empty;
            if (method == "BY_NAME")
            {
                Console.WriteLine("Введите имя:");
                fileName = Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Введите индефикатор:");
                fileName = Console.ReadLine();
            }

            writer.WriteLine($"DELETE {method} {fileName}");
            writer.Flush();

            string response = reader.ReadLine();
            Console.WriteLine(response);
        }
    }

    static void Exit()
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 8888))
        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream))
        using (StreamWriter writer = new StreamWriter(stream))
        {
            writer.WriteLine($"EXIT");
            writer.Flush();

            string response = reader.ReadLine();
            Console.WriteLine(response);
        }
    }
}
