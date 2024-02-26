using Server;

if (args[0] == "--db-work")
{
    while (true)
    {
        Console.WriteLine("\nChose a command\n" +
        "1 - Show tables\n" +
        "2 - Print table data\n" +
        "3 - Update table\n" +
        "4 - Exit\n");
        string chose = Console.ReadLine();
        if (chose == "1")
        {
            DataBase.ShowTables();
        }
        if (chose == "2")
        {
            DataBase.PrintTable();
        }
        if (chose == "3")
        {
            DataBase.InsertLine();
        }
        if (chose == "4")
        {
            return;
        }
    }
}

if (args[0] == "--run")
{
    RunServer();
}

else
{
    WriteHelpMessage();
}

void RunServer()
{
    int port = 4444;
    AsyncService service = new AsyncService(port);
    service.Run().GetAwaiter().GetResult();
}

static void WriteHelpMessage()
{
    Console.WriteLine("Server version 0.1 BETA\n" +
        "How use:\n" +
        "--run\n" +
        "--db-work");
}