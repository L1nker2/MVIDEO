namespace Server
{
    public class Test
    {
        public static void CodeInBase64(string path)
        {
            string base64 = Convert.ToBase64String(File.ReadAllBytes(path));
            Console.WriteLine(base64 + Environment.NewLine);

        }
    }
}
