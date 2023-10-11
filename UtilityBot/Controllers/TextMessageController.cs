using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Services;
using VoiceTexterBot.Services;


namespace VoiceTexterBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;
        private readonly SymbolsService _symbolsService;
        private readonly SummatorService _summatorService;
        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage, SymbolsService symbolsService, SummatorService summatorService)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
            _symbolsService = symbolsService;
            _summatorService = summatorService;
        }



        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"[{DateTime.Now}] Контроллер {GetType().Name} получил сообщение: {message.Text}");


            string userFunction = _memoryStorage.GetSession(message.Chat.Id).Function; // Здесь получим функцию бота из сессии пользователя


            switch (message.Text)
            {

                case "/function":
                   
                    userFunction = "not selected";
                    break;
                
                default:

                    break;
            }

            switch (userFunction)
            {
                case "Summator":

                    int sum = _summatorService.Summator(message);
                    if (sum != 0) 
                    { 
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел в сообщении - {sum}", cancellationToken: ct); 
                    }
                    else
                    {
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"В сообщении нет чисел.", cancellationToken: ct);
                    }
                    

                    break;

                case "Symbols":

                    int symbols = _symbolsService.Symbol(message);
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Символов в сообщении - {symbols}", cancellationToken: ct);
                    break;

                case "not selected":

                    // Объект, представляющий кнопки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Символы" , $"Symbols"),
                        InlineKeyboardButton.WithCallbackData($" Сумма" , $"Summator")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Выбери функцию бота.</b> {Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;

                default:
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Ничего не считаем", cancellationToken: ct);
                    break;

            }

        }


    }

}