# [0.3.0](https://github.com/m4cx/kraken-wsapi-dotnet/releases/tag/0.3.0) (2019-07-28)


### Bug Fixes

* **events:** add missing events to interface ([68600a1](https://github.com/m4cx/kraken-wsapi-dotnet/commit/68600a1))
* **heartbeat:** provide heartbeat event in interface ([b4fcfdc](https://github.com/m4cx/kraken-wsapi-dotnet/commit/b4fcfdc))
* **heartbeat:** recognize and publish heartbeat ([cb86dcb](https://github.com/m4cx/kraken-wsapi-dotnet/commit/cb86dcb)), closes [#29](https://github.com/m4cx/kraken-wsapi-dotnet/issues/29)
* capture heartbeat in sample ([7728a8d](https://github.com/m4cx/kraken-wsapi-dotnet/commit/7728a8d))
* fix unit tests ([e7c97a9](https://github.com/m4cx/kraken-wsapi-dotnet/commit/e7c97a9))


### Features

* **client:** introduce new client factory to create connections easily ([435a63f](https://github.com/m4cx/kraken-wsapi-dotnet/commit/435a63f)), closes [#22](https://github.com/m4cx/kraken-wsapi-dotnet/issues/22)
* **client:** support IDisposable pattern for client ([a6b4c1d](https://github.com/m4cx/kraken-wsapi-dotnet/commit/a6b4c1d)), closes [#23](https://github.com/m4cx/kraken-wsapi-dotnet/issues/23)
* **logging:** use logging abstraction ([be0cb85](https://github.com/m4cx/kraken-wsapi-dotnet/commit/be0cb85)), closes [#32](https://github.com/m4cx/kraken-wsapi-dotnet/issues/32)
* **pair:** provide constants for supported currentcy pairs ([3d226f2](https://github.com/m4cx/kraken-wsapi-dotnet/commit/3d226f2)), closes [#24](https://github.com/m4cx/kraken-wsapi-dotnet/issues/24)



# [0.2.0](https://github.com/m4cx/kraken-wsapi-dotnet/releases/tag/0.2.0) (2019-06-27)


### Bug Fixes

* **events:** add missing events to interface ([68600a1](https://github.com/m4cx/kraken-wsapi-dotnet/commit/68600a1))


### Features

* **client:** introduce new client factory to create connections easily ([435a63f](https://github.com/m4cx/kraken-wsapi-dotnet/commit/435a63f)), closes [#22](https://github.com/m4cx/kraken-wsapi-dotnet/issues/22)
* **client:** support IDisposable pattern for client ([a6b4c1d](https://github.com/m4cx/kraken-wsapi-dotnet/commit/a6b4c1d)), closes [#23](https://github.com/m4cx/kraken-wsapi-dotnet/issues/23)



# [0.1.1](https://github.com/m4cx/kraken-wsapi-dotnet/releases/tag/0.1.1) (2019-06-13)


### Bug Fixes

* support current websocket api version 0.2.0 ([173b466](https://github.com/m4cx/kraken-wsapi-dotnet/commit/173b466)), closes [#26](https://github.com/m4cx/kraken-wsapi-dotnet/issues/26)


# 0.1.0 (2019-04-11)

### Features

* **book:** handle book snapshot message from websocket ([baec641](https://github.com/m4cx/kraken-wsapi-dotnet/commit/baec641))
* **book:** handle bookupdate messages from websocket ([e56dc71](https://github.com/m4cx/kraken-wsapi-dotnet/commit/e56dc71))
* **ohlc:** handle ohlc messages from websocket ([a0817d7](https://github.com/m4cx/kraken-wsapi-dotnet/commit/a0817d7))
* **spread:** handle spread messages from websocket ([f46b305](https://github.com/m4cx/kraken-wsapi-dotnet/commit/f46b305))
* **subscribe:** create subscriptions to the api ([070f19d](https://github.com/m4cx/kraken-wsapi-dotnet/commit/070f19d))
* **system-status:** show system status ([d2d4650](https://github.com/m4cx/kraken-wsapi-dotnet/commit/d2d4650))
* **ticker:** extract ticker information for existing subscription ([0f32473](https://github.com/m4cx/kraken-wsapi-dotnet/commit/0f32473))
* **trade:** handle trade messages from websocket ([c9ef828](https://github.com/m4cx/kraken-wsapi-dotnet/commit/c9ef828))
* **unsubscribe:** unsubscribe from api based on provided channelId ([b832f72](https://github.com/m4cx/kraken-wsapi-dotnet/commit/b832f72))
* **websocket:** add simple websocket handler ([c3caa75](https://github.com/m4cx/kraken-wsapi-dotnet/commit/c3caa75))
