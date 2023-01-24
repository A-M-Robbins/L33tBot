using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeetBot
{
     class Program
    {
        System.Timers.Timer timer = new(interval: 1000);
        DiscordSocketClient? _client;
        private CommandService? _commands;
        private IServiceProvider? _services;
        static async Task Main(string[] args)
        {
            Program pro = new Program();
            await pro.RunBotAsync();

        }


         async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
             _commands = new CommandService();
            var config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.All
            };
            _client = new DiscordSocketClient(config);
            string token = "MTA0NTE1MDk2ODcxMjUzMTk5OQ.G8Lku0.dbBWwfBsx2y2wh_Z-qQ39y9FeqZBRNcmOmpCaE";
            _client.Log += _client_Log;
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }


        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleMessageAsync1;
             await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task HandleMessageAsync1(SocketMessage arg)
        {
            Console.WriteLine("At the beginning of HandleMessageAsync1");
            var message = arg as SocketUserMessage;
            var user = message.Author;
            string username = user.Username;
            string discriminator = user.Discriminator;
            var context = new SocketCommandContext(_client, message);
            if (arg.Content.StartsWith("handleOne"))
            {
                _client.MessageReceived -= HandleMessageAsync1; //SUBTRACT FROM MESSAGERECEIVED so no repeat
                string R = AskRandomQuestion();

                Console.WriteLine("The person that met the 'if statement requirements' is " + username);
                Console.WriteLine("Made it through 'If statement 1' in 'HandleMessageAsync1' ");
                Console.WriteLine("Moving on to 'HandleMessageAsync2' ");

                
                await context.Channel.SendMessageAsync("```Ok " + username + " let's get started \nHere's a question" + "```");
                await context.Channel.SendFileAsync($"{R}", "Can you solve this?");
                DateTime startTime = DateTime.Now;
                // Call HandleMessageAsync2 asynchronously
                await Task.Run(() => HandleMessageAsync2(message));

            }

            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine("It no work! \nIt no work because: " + result.ErrorReason); //If the commands dont execute due to an error it will tell you why in the console making debugging easier
                if (result.Error.Equals(CommandError.UnmetPrecondition)) await message.Channel.SendMessageAsync(result.ErrorReason);
            }

        }



        public Task HandleMessageAsync2(SocketMessage arg)// <-----Runs SYNCHRONOUSLY
        {
            _client.MessageReceived -= HandleMessageAsync2; //SUBTRACT FROM MESSAGERECEIVED so no repeat
            _client.MessageReceived += HandleMessageAsync2;//ADD TO MESSAGERECEIVED 
            List<double> answerSpeed = new List<double>();
            DateTime startTime = DateTime.Now;
            var message = arg as SocketUserMessage;
            var user = message.Author;
            string username = user.Username;
            string discriminator = user.Discriminator;
            var context = new SocketCommandContext(_client, message);
            int argPos = 0;
                Console.WriteLine("At the beginning of HandleMessageAsync2");
            if (message.Author.IsBot && arg.Content.StartsWith("answer")) 
            {
                Console.WriteLine("Error! Bots cannot answer questions, please answer again");
            }

            if (arg.Content.StartsWith("answer"))
            {
                DateTime answerTime = DateTime.Now;
                double answer1 = (answerTime.Subtract(startTime).TotalMinutes);

                Console.WriteLine("Made it through inner if statement!!!");
                Console.WriteLine(username + " Entered the correct answer in " + answer1 + " Minutes");
                Console.WriteLine(username + "Storing data...");


                context.Channel.SendMessageAsync("**Correct!!!**");
                context.Channel.SendMessageAsync("```diff\n +1\n``` ");
                context.Channel.SendMessageAsync("```You answered this question in " + answer1 + " Minutes```");
                answerSpeed.Add(answer1);


            }

            return Task.CompletedTask;
        }


        static string AskRandomQuestion()
        {
            string[] arrayType = Directory.GetFiles("C:\\Users\\andre\\OneDrive\\Desktop\\Blind_Top_75\\Array");

            Random rand = new Random();
            for (int i = 0; i < arrayType.Length - 1; i++)
            {
                int r = rand.Next(i, arrayType.Length);
                (arrayType[r], arrayType[i]) = (arrayType[i], arrayType[r]);
            }
            string askRandQ = arrayType[1];
            return askRandQ;
        }
    }
}