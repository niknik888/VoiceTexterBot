using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using VoiceTexterBot.Controllers;
using VoiceTexterBot.Services;
using VoiceTexterBot.Configuration;
using UtilityBot.Services;

namespace VoiceTexterBot
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine($"[{DateTime.Now}] Сервис запущен");
            // Запускаем сервис
            await host.RunAsync();
            Console.WriteLine($"[{DateTime.Now}] Сервис остановлен");
        }


        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<DefaultMessageController>();

            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();

            // Регистрируем объект TelegramBotClient c токеном подключения
            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            // Регистрируем постоянно активный сервис бота
            services.AddHostedService<Bot>();
            // Регистрируем хранилище сессий
            services.AddSingleton<IStorage, MemoryStorage>();
            services.AddSingleton<SymbolsService>();
            services.AddSingleton<SummatorService>();



        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "6383723323:AAGjEgRMV81yHBEmt4hGru9QoIFXTrlJLjM",

            };
        }
    }
}
