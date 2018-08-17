# AADv2Sample-Workarounds
Shows a number of workarounds for specific channels when using the AADv2 sample

You will need to have your packages folder 2 levels up from the .csproj file 

-----

•	**Twilio SMS**

  o	Unable to complete Oauth flow '+' character in URL does not allow it
  
  •	Workaround in this issue https://github.com/Microsoft/BotBuilder/issues/4880 
 
  o	On iOS the link to login does not display as a hyperlink in its entirety results in broken link.
  
  •	Workaround here https://github.com/Microsoft/BotBuilder/issues/4897
 
•	**Kik**

  o	Cards in Kik do not support the openURL action type.
  
  •	This workaround fixes this as well https://github.com/Microsoft/BotBuilder/issues/4897 
 
•	**Telegram**

  o	When you click the signin button on iOS it dispays a long link which you cannot click on or copy.  On android it displays the same, but you can scroll down to click on an "open" button.  This button is unreachable on iOS
  
  •	This workaround fixes this https://github.com/Microsoft/BotBuilder/issues/4897
 
•	**Teams**

  o	Unable to click on the signin card button because teams does not support the SIgnIn ActionType.
  
  •	Workaround in this issue https://github.com/Microsoft/BotBuilder/issues/4768 Note: would also be fixed by the workaround in this issue https://github.com/Microsoft/BotBuilder/issues/4897
  
  o	When the magic code is copied and pasted into the chat it adds extra characters like "\r\n123456\r"
  
  •	Workaround in this issue https://github.com/Microsoft/BotBuilder/issues/4899
 
•	**Slack**

  o	Unable to complete the full Oauth flow due to the token not being returned after inputting the magic code
  •	This workaround fixes this https://github.com/Microsoft/BotBuilder/issues/4897
 
•	**Facebook**

  o	Observed no issues
 
•	**Skype**

  o	Observed no issues
 
•	**GroupMe**

  o	Breaks the link into 2 messages making it a broken link
  
  •	This workaround fixes this https://github.com/Microsoft/BotBuilder/issues/4897
 
•	**WebChat**

  o	Observed no issues
 
•	**Email**

  o	Unable to get a response always gateway timeout.
 
•	**Cortana**

  Did not test

•	**Skype for Business**

  Did not test
