Linux prerequisites:

```sudo apt-get install clang zlib1g-dev libkrb5-dev libtinfo5```

To build for linux:

```dotnet publish -r linux-x64 -c Release```

Strip is required to get rid of extra debug info on linux

```strip bin/Release/net5.0/linux-x64/publish/CoreRTSample```