mkdir build\gen
protoc --proto_path=. --csharp_out=build/gen SerialTempSens_messages.proto
