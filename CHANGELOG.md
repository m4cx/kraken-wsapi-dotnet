# [0.5.0](https://github.com/m4cx/kraken-wsapi-dotnet/compare/0.4.0...0.5.0) (2020-04-06)

### Features

* **add-order:** send addOrder message through websocket ([8ab6241](https://github.com/m4cx/kraken-wsapi-dotnet/commit/8ab6241))
* **add-order-status:** handle and publish addOrderStatus event ([166eed2](https://github.com/m4cx/kraken-wsapi-dotnet/commit/166eed2))
* **cancel-order-status:** handle and publish response event ([c6c3630](https://github.com/m4cx/kraken-wsapi-dotnet/commit/c6c3630))
* **cancel-order:** send cancel order command via websocket ([f774962](https://github.com/m4cx/kraken-wsapi-dotnet/commit/f774962))
* **open-orders:** handle open order messages from websocket ([7fcb607](https://github.com/m4cx/kraken-wsapi-dotnet/commit/7fcb607))


# [0.4.0](https://github.com/m4cx/kraken-wsapi-dotnet/compare/0.3.0...0.4.0) (2020-03-29)

### Bug Fixes

* **logging:** use lock for multi threading ([749e1c7](https://github.com/m4cx/kraken-wsapi-dotnet/commit/749e1c7))
* **subscription-status:** surpress error when subscribing without token ([c86daca](https://github.com/m4cx/kraken-wsapi-dotnet/commit/c86daca))


### Features

* **authentication:** request authentication token from rest api ([43c3d7d](https://github.com/m4cx/kraken-wsapi-dotnet/commit/43c3d7d)), closes [#41](https://github.com/m4cx/kraken-wsapi-dotnet/issues/41)
* **open-orders:** handle open order messages from websocket ([7fcb607](https://github.com/m4cx/kraken-wsapi-dotnet/commit/7fcb607))
* **own-trades:** deserializing data from socket ([416ead7](https://github.com/m4cx/kraken-wsapi-dotnet/commit/416ead7))
* **private-message:** separate logic for private data from data ([2e47eb7](https://github.com/m4cx/kraken-wsapi-dotnet/commit/2e47eb7))
* **subscription:** provide authentication token for private data ([34c658f](https://github.com/m4cx/kraken-wsapi-dotnet/commit/34c658f))


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
