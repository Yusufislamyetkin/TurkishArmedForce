# Turkish Armed Force Messaging System

This repository contains the base solution skeleton for the messaging system.

## Getting Started

1. Ensure the .NET 8 SDK is installed.
2. Restore and build the solution:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Run the web API:
   ```bash
   dotnet run --project src/Messaging.Api
   ```
4. Other applications such as the realtime hub, background worker, gateway and auth server can be executed with `dotnet run` targeting their respective project files.
