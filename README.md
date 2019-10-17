| master                                                                                                                                                                                                                                | develop                                                                                                                                                                                                                                 | nuget                                                                                                               |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------- |
| [![Build Status](https://macx.visualstudio.com/kraken-wsapi-dotnet/_apis/build/status/m4cx.kraken-wsapi-dotnet?branchName=master)](https://macx.visualstudio.com/kraken-wsapi-dotnet/_build/latest?definitionId=12&branchName=master) | [![Build Status](https://macx.visualstudio.com/kraken-wsapi-dotnet/_apis/build/status/m4cx.kraken-wsapi-dotnet?branchName=develop)](https://macx.visualstudio.com/kraken-wsapi-dotnet/_build/latest?definitionId=12&branchName=develop) | [![Stable](https://img.shields.io/nuget/v/Kraken.WebSockets.svg)](https://www.nuget.org/packages/Kraken.WebSockets) |

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
```
```bash
> dotnet add package Kraken.WebSockets # .NET CLI
```

For detailed information on the installing of pre-release versions please refer to the [NuGet Gallery](https://www.nuget.org/packages/Kraken.WebSockets) itself.

## Create a connection and listen for events

Creating a connection is pretty easy but will also be improved in the future. But for now just do it like this:

```csharp
var krakenApi = new KrakenApi()
    .ConfigureWebsocket("wss://ws.kraken.com");

using (var client = krakenApi.BuildClient())
{
    client.SystemStatusChanged += (sender, e) => Console.WriteLine($"System status changed");
    client.SubscriptionStatusChanged += (sender, e) => Console.WriteLine($"Subscription status changed"); ;
    client.TickerReceived += (sender, e) => Console.WriteLine($"Ticker received");
    client.OhlcReceived += (sender, e) => Console.WriteLine($"Ohlc received");
    client.TradeReceived += (sender, e) => Console.WriteLine($"Trade received");
    client.SpreadReceived += (sender, e) => Console.WriteLine($"Spread received");
    client.BookSnapshotReceived += (sender, e) => Console.WriteLine($"BookSnapshot received");
    client.BookUpdateReceived += (sender, e) => Console.WriteLine($"BookUpdate received");

    await kraken.ConnectAsync();
    // Do something with it and keep the connection open
}
// closing the using-block the connection will be closed and disposed.
```

You can also find a running example in the repository.

## Subscribe to private events

Starting with their version 0.3.0 of the Websocket API kraken.com provides access to sensitive private account information like trades and orders. In order to gain access you need to [authenticate](https://www.kraken.com/features/websocket-api#authentication).

We support an easy and feasible way to retrieve a token and pass it to the subscription:

```csharp

// Configure the API provider
var krakenApi = new KrakenApi()
    .ConfigureWebsocket("wss://ws.kraken.com")
    .ConfigureAuthentication(
        configuration.GetValue<string>("API_URL"),
        configuration.GetValue<string>("API_KEY"),
        configuration.GetValue<string>("API_SECRET"));

// retrieve the token
var token = await krakenApi.AuthenticationClient.GetWebsocketToken();

// Pass the token to a private subscription
await client.SubscribeAsync(new Subscribe(null, new SubscribeOptions(SubscribeOptionNames.OwnTrades, token.Token)));

```

## Attach your logging framework

With the `Microsoft.Extensions.Logging.Abstractions` in place you are not limited to a specific logging framework. Each logging framework which supports this abstraction layer can be attached to this library. Just to name a few of them:

- log4net
- Serilog
- NLog
- ... and many more

To enable the logging just call the provided extension method on your `ILoggerFactory` of your application.

```csharp
// Using the logging from ASP.NET Core MVC
// Startup.cs

using Kraken.WebSockets.Logging;

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    // ... source omitted

    loggerFactory.AddKrakenWebSockets(); // Enables the configured logging factory for the logs in Kraken.WebSockets

    // ... source omitted
}

```

# Support

If you like the stuff I do, please don't hesitate to support my actions by donating me a coffee!

<a class="bmc-button" target="_blank" href="https://www.buymeacoffee.com/rkqS0BIKu"><img src="https://www.buymeacoffee.com/assets/img/BMC-btn-logo.svg" alt="Buy me a coffee"><span style="margin-left:5px">Buy me a coffee</span></a><br/><br/><a href="https://www.paypal.me/maikschoeneich"><img src="https://www.paypalobjects.com/webstatic/de_DE/i/de-pp-logo-100px.png"/>-Me</a>
