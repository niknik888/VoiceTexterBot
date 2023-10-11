using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using VoiceTexterBot.Models;

namespace UtilityBot.Services
{
    public class SummatorService
    {
        public int Summator(Message message)
        {

           
                string[] numberStrings = message.Text.Split(' '); // Разбиваем строку на числа
                int sum = 0;

                foreach (string numberString in numberStrings)
                {
                    if (int.TryParse(numberString, out int number))
                    {
                        sum += number;
                    }
                    else
                    {
                    Console.WriteLine($"[{DateTime.Now}] Ошибка: Не удалось преобразовать '{numberString}' в число.");
                    }
                }

                return sum;
            }
        
    }
}
