using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace YourBotNamespace
{
    internal class Program
    {
        private static DiscordSocketClient _client;
        private static Dictionary<ulong, string> userStates = new Dictionary<ulong, string>();
        private static Dictionary<ulong, DateTime> lastResponseTimes = new Dictionary<ulong, DateTime>();
        private static Dictionary<ulong, int> lastResponseIndices = new Dictionary<ulong, int>(); // To store last response indices
        private static List<string> enesFacts = new List<string>(); // Initialize an empty list for Enes's facts
        private static List<string> enesFactsShuffled = new List<string>(); // Shuffled list for Enes's facts
        private static ulong specificUserId = 147820834965291008; // Replace with the specific user's ID.
        private static TimeSpan responseCooldown = TimeSpan.FromMinutes(10);

        public Program()
        {
            InitializeEnesFacts(); // Initialize and shuffle Enes's facts at startup
        }

        private static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        public static async Task MainAsync()
        {
            var socketConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.GuildPresences | GatewayIntents.MessageContent
            };

            _client = new DiscordSocketClient(socketConfig);

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;

            var token = "MTIwNDUyMzM3NzA0NjQ1ODM3OA.GxYbbU.dAoUMmxgmsh1ZdMUt3aW-lf5QC6W8MFojZeHxg"; // Replace with your bot's token.

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private static Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private static Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");
            return Task.CompletedTask;
        }

        private static void InitializeEnesFacts()
        {
            enesFacts = new List<string>
            {
                // Add your facts here
                "**Enes is only menace in the morning hours**",
                "**Behind the tough shell of his toxicity, there is a loving and caring man behind.**",
                "**Enes houdt er van om in conversaties te jumpen als er een discussie is** ",
                "**Enes houdt van de dames maar kan een Thicc Ebony Babe niet weerstaan.**",
                "**Enes zn favoriete artiest Kid Cudi heeft inspiratie genomen van de Liedje : Aksam Gunduz.**",
                "**Als er iets niet goed gaat in een game, dan is het altijd jou schuld.**",
                "**Enes heeft heel weinig lichaams haar.**",
                "**Als je eigen osso hebt is het de osso van Enes.**",
                "**Enes heeft geen tafel manieren bij het eten van booty.**",
                "**Enes is net als een thermostaat, als het wordt overloaded barst hij uit, maar al gauw koelt hij af.**",
                "**Enes vroeg een keertje : Speel je Runescape of RS ?**",
                "**Soms zit Enes alleen in call zodat iemand hem randomly joined en een gesprek mee aan gaat.**",
                "**Enes speelt graag mannen charracters en verafschuwt female charracters.**",
                "**Enes soms camide kufur ediyor.**",
                "**Enes heeft wel eens djonko aan hoca verkocht.**",
                "**Enes heeft een kromme lul.**"
            };
            enesFactsShuffled = enesFacts.OrderBy(x => Guid.NewGuid()).ToList(); // Shuffle the facts
        }

        private static async Task MessageReceivedAsync(SocketMessage message)
        {
            if (!(message is IUserMessage userMessage)) return;
            if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot) return;

            if (message.Content == "!nova")
            {
                // Provide information about the app
                string appInfo = "Random fact over Enes";

                await message.Channel.SendMessageAsync(appInfo);

                if (!enesFactsShuffled.Any())
                {
                    // All facts have been shown, re-shuffle
                    InitializeEnesFacts();
                }

                string fact = enesFactsShuffled.First(); // Get the first fact
                await message.Channel.SendMessageAsync(fact);
                enesFactsShuffled.RemoveAt(0); // Remove the shown fact
                return;
            }

            // Existing logic for specific user responses
            if (message.Author.Id == specificUserId)
            {
                if (!lastResponseTimes.TryGetValue(specificUserId, out var lastResponse) || (DateTime.UtcNow - lastResponse) > responseCooldown)
                {
                    var responses = new List<string>
                    {
                        "Enes anlamadin mi oglum, er is niks te zoeken hier man..",
                        "Yaw.. kac kere demeliyim..yok yok yok.. yeter ya..",
                        "Nee man sorry, er is hier echt niks te vinden.",
                        "Tamam tamam geldin baktin discord. Simdi geri git, birsey yok burda..",
                        "Eeee ? baktin simdi ne var ? geen drama. Hadi yallah kapat discord.",
                        "Oke man ook hosgeldin sana. okuncak birsey yok burda.",
                        "Oooo hey Enes! Ja man klopt.. Vandaag weer niks te vinden op discord gozer !",
                        "Tazzz adam hala yaziyor burda",
                        "Kijk ama simdi yeter ya..",
                        "Gotu boklu seni",
                        "OK Leuk Man, Leuk dat je dit deelt met ons.Napalim simdi ?",
                        "Seni pis pis.. varya Enes.. Ja.. Aynen.. beter stil zijn",
                        "WEEE WOOO WEEE WOOO, Ja hoor Enes heeft weer wat te melden ! Leuk !",
                        "Ya sikerim ha.. duzgun dur",
                        "Soms seni anlamiyorum man..smh..",
                        "Anlamadim,  je praat nogsteeds ?",
                        "Als er iemand was wie zn haren ik kon trekken dan is dat die van Enes wel",
                    };

                    int randomIndex;
                    Random rnd = new Random();
                    do
                    {
                        randomIndex = rnd.Next(responses.Count);
                    } while (lastResponseIndices.TryGetValue(specificUserId, out var lastIndex) && randomIndex == lastIndex && responses.Count > 1);

                    var selectedResponse = responses[randomIndex];
                    await message.Channel.SendMessageAsync(selectedResponse);
                    lastResponseTimes[specificUserId] = DateTime.UtcNow;
                    lastResponseIndices[specificUserId] = randomIndex;
                }
            }
        }
    }
}
