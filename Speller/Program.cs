using System;
using System.IO;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Tls;
using MySql.Data.MySqlClient;

string connectionString;

string pathToFile, tableName, serverName, databaseName, userName, password, sure;
int correct = 0;
int incorrect = 0;
connectionString = "Server=localhost;Database=Dictionary;User ID=root;";
tableName = "Words";
pathToFile = "/Users/nikitagavrilov/Downloads/texts/wordsworth.txt";
string content = File.ReadAllText(pathToFile);
string[] words = Regex.Split(content, @"\W+", RegexOptions.Compiled);

try
{
    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        connection.Open();
        foreach(string word in words)
        {
            //Console.WriteLine(word);
            string query = $"SELECT `word` FROM `{tableName}` WHERE `word` = (@word) LIMIT 1;";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@word", word);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        correct++;
                    }
                    else
                    {
                        incorrect++;
                    }
                }
            }
            if(((correct + incorrect) % 10) == 0)
            {
                Console.WriteLine(correct + incorrect);
            }
        }
        Console.WriteLine($"In this text num of \n\tcorrect words: {correct}\n\tincorrect words: {incorrect}");
    }
}
catch (Exception ex)
{
    Console.WriteLine("An error appeared: " + ex.Message);
}

