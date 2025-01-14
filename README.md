A visual studio extension that edds a menu to open 'My first tool window' and in the tool window embeds WebView2 with a react app.
in the web view click send message to extension, the extension will reply by showing an alert with a message.



There is a tool window that loads a react app and communicates with the react app.
in the experimental instance go to view -> other windows -> My first tool window


there are extension settings.
go to Tools -> Options , search for digma 


there is a commend  Tools -> invoke MyToolOptionsCommand
it will show the value saved in digma page in the settings

a command Tools -> Open page command
it will open the options page and focus on digma settings