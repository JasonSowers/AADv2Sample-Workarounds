using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;


namespace Microsoft.Bot.Sample.AadV2Bot
{
    public static class CustomCode
    {
        public static Dictionary<string, string> RedirectDictionary = new Dictionary<string, string>();
        public static string SanatizeMagicCodeForTeams(string magicCode)
        {
            Regex regex = new Regex(@"\r\n(\d{6})\n");
            var match = regex.Match(magicCode);
            if (match.Success)
            {
                magicCode = magicCode.Substring(2, 6);
            }
            return magicCode;
        }
    }
    public sealed class TeamsActivityMapper : IMessageActivityMapper
    {
        public IMessageActivity Map(IMessageActivity message)
        {
            if (message.ChannelId == ChannelIds.Msteams)
            {
                if (message.Attachments.Any() && message.Attachments[0].ContentType == "application/vnd.microsoft.card.signin")
                {
                    var card = message.Attachments[0].Content as SigninCard;
                    var buttons = card.Buttons as CardAction[];
                    if (buttons.Any())
                    {
                        buttons[0].Type = ActionTypes.OpenUrl;
                    }
                }
            }
            return message;
        }
    }
}
