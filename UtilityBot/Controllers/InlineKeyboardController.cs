using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VoiceTexterBot.Services;

namespace VoiceTexterBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).Function = callbackQuery.Data;
            Console.WriteLine($"[{DateTime.Now}] Контроллер {GetType().Name} обнаружил нажатие на кнопку: {callbackQuery.Data}");

            // Генерим информационное сообщение
            string selectedFunction = callbackQuery.Data switch
            {

                "Symbols" => " Символы",
                "Summator" => " Сумма",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе

            switch (selectedFunction)
            {
                case " Символы":

                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                    $"<b>Функция - {selectedFunction}.{Environment.NewLine}</b>" +
                    $"Я посчитаю количество символов в твоем сообщении" +
                    $"{Environment.NewLine}Можно поменять командой /function", cancellationToken: ct, parseMode: ParseMode.Html);
                    break;

                case " Сумма":

                    await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                    $"<b>Функция - {selectedFunction}.{Environment.NewLine}</b>" +
                    $"Я посчитаю сумму чисел, введенных через пробел в твоем сообщении" +
                    $"{Environment.NewLine}Можно поменять командой /function", cancellationToken: ct, parseMode: ParseMode.Html);
                    break;
            }

        }
    }
}
