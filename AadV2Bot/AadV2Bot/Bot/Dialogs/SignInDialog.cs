using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Bot.Sample.AadV2Bot.Dialogs
{
    [Serializable]
    public class SignInDialog : IDialog<GetTokenResponse>
    {
        private string _connectionName;
        private string _buttonLabel;
        private string _signInMessage;
        private int _retries;
        private string _retryMessage;
        private Guid _guid;

        public SignInDialog(string connectionName, string signInMessage, string buttonLabel, int retries = 0,
            string retryMessage = null)
        {
            _connectionName = connectionName;
            _signInMessage = signInMessage;
            _buttonLabel = buttonLabel;
            _retries = retries;
            _retryMessage = retryMessage;
        }

        public async Task StartAsync(IDialogContext context)
        {
            // First ask Bot Service if it already has a token for this user
            var token = await context.GetUserTokenAsync(_connectionName);
            if (token != null)
            {
                context.Done(new GetTokenResponse() {Token = token.Token});
            }
            else
            {
                // If Bot Service does not have a token, send an OAuth card to sign in
                await SendOAuthLinkAsync(context, (Activity) context.Activity);
            }
        }

        private async Task SendOAuthLinkAsync(IDialogContext context, Activity activity)
        {
            var reply = activity.CreateReply();
            _guid = Guid.NewGuid();

            var client = activity.GetOAuthClient();
            var link = await client.OAuthApi.GetSignInLinkAsync(activity, _connectionName);
            
            CustomCode.RedirectDictionary.Add("/api/redirect?" +_guid, link);
            var url = $"{YOUR_BOTS_ENDPOINT}/api/redirect?{_guid}";

            // these channels have some limitation on cards and require you to send a link instead
            if (activity.ChannelId == ChannelIds.Sms 
                || activity.ChannelId == ChannelIds.Kik
                || activity.ChannelId == ChannelIds.Groupme)
            {
                var textReply =
                    activity.CreateReply("Use this link to get a magic code.  Enter the magic code here to continue");
                await context.PostAsync(textReply);
                reply.Text =url;
            }
            else
            {
                reply.Attachments= new List<Attachment>()
                {
                    new HeroCard(
                        title: "Sign in",
                        text: "Click this button to get your magic code.\r\n Enter the magic code to continue",
                        buttons: new List<CardAction>()
                        {
                            new CardAction(text:"Get Code",type: ActionTypes.OpenUrl, title:"Get Code", value: url, displayText:"Get Code")
                        }
                        ).ToAttachment()
                };
            }
            await context.PostAsync(reply);

            context.Wait(WaitForToken);
        }

        private async Task WaitForToken(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            CustomCode.RedirectDictionary.Remove(_guid.ToString());
            var tokenResponse = activity.ReadTokenResponseContent();
            string verificationCode = null;
            if (tokenResponse != null)
            {
                context.Done(new GetTokenResponse() {Token = tokenResponse.Token});
                return;
            }
            else if (activity.IsTeamsVerificationInvoke())
            {
                JObject value = activity.Value as JObject;
                if (value != null)
                {
                    verificationCode = (string) (value["state"]);
                }
            }
            else if (!string.IsNullOrEmpty(activity.Text))
            {
                verificationCode = activity.Text;
            }

            tokenResponse = await context.GetUserTokenAsync(_connectionName, verificationCode);
            if (tokenResponse != null)
            {
                context.Done(new GetTokenResponse() {Token = tokenResponse.Token});
                
                return;
            }

            // decide whether to retry or not
            if (_retries > 0)
            {
                _retries--;
                await context.PostAsync(_retryMessage);
                await SendOAuthLinkAsync(context, activity);
            }
            else
            {
                context.Done(new GetTokenResponse() {NonTokenResponse = activity.Text});
                return;
            }
        }
    }
}