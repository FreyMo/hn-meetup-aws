FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /build
COPY . .

# Uncomment this line when building for linux/amd64 on Apple Silicon
# This workaround is required because .NET does not properly support QEMU
# ENV DOTNET_EnableWriteXorExecute=0

RUN dotnet publish "Aws.Meetup.WebApi.csproj" -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aws.Meetup.WebApi.dll"]
