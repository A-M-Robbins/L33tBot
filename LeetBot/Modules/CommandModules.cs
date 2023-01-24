using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetBot.Modules
{
    public static class CommandModules
    {
        private static SocketCommandContext? _Context;

        public static string ShowLeaderBoard(Dictionary<string, int> sortedDict)
        {
            string s = "";
            int i = 0;
            Console.WriteLine("======================================");
            Console.WriteLine("Current leaderboard: ");
            foreach (KeyValuePair<string, int> entry in sortedDict)
            {
                i++;

               s += i + ". " + entry.Key + " " + entry.Value + " " + "\n";
            }
            Console.WriteLine("======================================");
            Console.WriteLine(s);
            return s;
        }


        public static string ReadFileFrmTextToDict()
        {
            //*******************Start Read
            StreamReader reader = new StreamReader("C:\\Users\\andre\\OneDrive\\Desktop\\VisualStudioFiles\\ReadableFiles\\Dbot_LeaderBoard.txt");
            Dictionary<string, int> unsortedDict = new Dictionary<string, int>();
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] parts = line.Split('-');
                string key = parts[0].Trim();
                int value = int.Parse(parts[1].Trim());
                unsortedDict.Add(key, value);
            }
            reader.Close();
            //End Read**************
            //*******************Sorting dictionary
            var sortedDict = unsortedDict.OrderByDescending(kvp => kvp.Value)
                                 .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            //End sort**************
            return ShowLeaderBoard(sortedDict);
            // AddToUserLBoardScore(sortedDict);
        }


    }

    public class CommandsModule : ModuleBase<SocketCommandContext>
    {
        [Command("leaderboard")]
        public async Task Ping()
        {
            
            await ReplyAsync("The Current Leaderboard is \n" + " \"🤖\"  " + "\n" + CommandModules.ReadFileFrmTextToDict());
        }
        [Command("start")]
        public async Task StartMessageTimer()
        {
            await ReplyAsync("Ok. . .Starting Timer");
        }
    }
}
