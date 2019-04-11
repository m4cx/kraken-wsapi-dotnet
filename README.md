|master|develop|nuget|
|------|-------|------|
|[![Build Status](https://macx.visualstudio.com/kraken-wsapi-dotnet/_apis/build/status/m4cx.kraken-wsapi-dotnet?branchName=master)](https://macx.visualstudio.com/kraken-wsapi-dotnet/_build/latest?definitionId=12&branchName=master)|[![Build Status](https://macx.visualstudio.com/kraken-wsapi-dotnet/_apis/build/status/m4cx.kraken-wsapi-dotnet?branchName=develop)](https://macx.visualstudio.com/kraken-wsapi-dotnet/_build/latest?definitionId=12&branchName=develop)| [![Stable](https://img.shields.io/nuget/v/Kraken.WebSockets.svg)](https://www.nuget.org/packages/Kraken.WebSockets)|

# kraken-wsapi-dotnet
C# / .NET Standard Client for connecting to the public kraken.com WebSocket API

# API Documentation
See the official documentation fur further information
https://www.kraken.com/features/websocket-api

# Getting started

## Installing it in your project environment
Get the current version from NuGet Gallery using the Package Manager / .NET CLI
```bash
PM> Install-Package Kraken.WebSockets # Package Manager
> dotnet add package Kraken.WebSockets # .NET CLI
```
For detailed information on the installing of pre-release versions please refer to the [NuGet Gallery](https://www.nuget.org/packages/Kraken.WebSockets) itself.

## Create a connection and listen for events
Creating a connection is pretty easy but will also be improved in the future. But for now just do it like this:
```csharp
 var uri = "wss://ws-sandbox.kraken.com";
var serializer = new KrakenMessageSerializer();
kraken = new KrakenWebSocket(uri, serializer);

var client = new KrakenApiClient(kraken, serializer);

client.SystemStatusChanged += (sender, e) => Console.WriteLine($"System status changed");
client.SubscriptionStatusChanged += (sender, e) => Console.WriteLine($"Subscription status changed"); ;
client.TickerReceived += (sender, e) => Console.WriteLine($"Ticker received");
client.OhlcReceived += (sender, e) => Console.WriteLine($"Ohlc received");
client.TradeReceived += (sender, e) => Console.WriteLine($"Trade received");
client.SpreadReceived += (sender, e) => Console.WriteLine($"Spread received");
client.BookSnapshotReceived += (sender, e) => Console.WriteLine($"BookSnapshot received");
client.BookUpdateReceived += (sender, e) => Console.WriteLine($"BookUpdate received");
await kraken.ConnectAsync();
```
You can also find a running example in the repository.

# Support
If you like the stuff I do, please don't hesitate to support my actions by donating me a coffee!

<a class="bmc-button" target="_blank" href="https://www.buymeacoffee.com/rkqS0BIKu"><img src="https://www.buymeacoffee.com/assets/img/BMC-btn-logo.svg" alt="Buy me a coffee"><span style="margin-left:5px">Buy me a coffee</span></a><br/><br/><a href="https://www.paypal.me/maikschoeneich"><img src="https://www.paypalobjects.com/webstatic/de_DE/i/de-pp-logo-100px.png"/>-Me</a>
