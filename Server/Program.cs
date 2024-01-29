using Server;
//C:\USERS\USER\DOCUMENTS\MVIDEO.MDF
static void Main(string[] args)
{
    if (args.Length == 0)
    {
        WriteHelpMessage();
    }
    else if (args.Length == 1) {
        if (args[0] == "--run-server")
        {
            int port = 4444;
            AsyncService service = new AsyncService(port);
            service.Run();
        }
        if (args[0] == "-h")
        {
            WriteHelpMessage();
        }
        if (args[0] == "--db-work")
        {
            Console.WriteLine("\nChose a command\n" +
                "1 - Show tables\n" +
                "2 - Print table data\n" +
                "3 - Update table\n");
            string chose = Console.ReadLine();
            if(chose == "1")
            {
                DataBase.ShowTables();
            }
            if(chose == "2")
            {

            }
            if(chose == "3")
            {

            }
            
        }
    }
}

static void WriteHelpMessage()
{
    Console.WriteLine("Server version 0.1 BETA\n" +
        "How use:\n" +
        "-h - print this message\n" +
        "--db-work - run work with DB mode\n" +
        "--run-server - run server ;-:");
}