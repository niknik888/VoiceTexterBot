using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using VoiceTexterBot.Models;

namespace UtilityBot.Services
{
    public class SymbolsService
    {
        public int Symbol(Message message)
        {

                return message.Text.Length;
        }
    }
}
