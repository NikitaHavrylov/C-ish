using Org.BouncyCastle.Tls;
using MySql.Data.MySqlClient;

string connectionString;


Console.WriteLine("\n\n\nHello, this is from Dictionary to SQL app\nAlso, prepare server name, database name, user and password");

string pathToFile, tableName, serverName, databaseName, userName, password, sure;
string[] words;

//Collect info
    Console.Write("\nPath to the file: ");
    pathToFile = Console.ReadLine();
    Console.Write("Table to use: ");
    tableName = Console.ReadLine();
    Console.Write("Server: ");
    serverName = Console.ReadLine();
    Console.Write("Database: ");
    databaseName = Console.ReadLine();
    Console.Write("User: ");
    userName = Console.ReadLine();

    string content = File.ReadAllText(pathToFile);
    words = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
    //Console.WriteLine(content);

do
{
    Console.Write("Password: ");
    password = Console.ReadLine();

    if(string.IsNullOrEmpty(password))
    {
        Console.WriteLine("Do you don't really need it? (Y/N)");
        sure = Console.ReadLine();
    }
    else
    {
        sure = "Y";
    }
}while(string.IsNullOrEmpty(sure) || sure.ToUpper() != "Y");
//\\Collect info

//Work with database
if(string.IsNullOrEmpty(password))
{
    connectionString = string.Format("Server={0};Database={1};User ID={2};", serverName, databaseName, userName);
}
else
{
    connectionString = string.Format("Server={0};Database={1};User ID={2};Password={3};", serverName, databaseName, userName, password);
}
try
{
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        connection.Open();
        // Console.WriteLine(connection);
        // Console.WriteLine(connectionString);
        int id = 0;
        foreach(string word in words)//insert dictionary
        {
            string query = $"INSERT INTO `{tableName}`(`word`) VALUES (@word)";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@word", word);
                command.ExecuteNonQuery();
            }
            id++;
        }
        Console.WriteLine($"{id} rows inserted");
        
    }
}
catch (Exception ex)
{
    Console.WriteLine("An error appeared: " + ex.Message);
}
//\\Work with database