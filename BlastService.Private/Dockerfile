FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS https://+:61901
EXPOSE 61901

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY ["BlastService.Private.csproj", "./"]
COPY ["BlastService.Private.ModelContract/BlastService.Private.ModelContract.csproj", "BlastService.Private.ModelContract/"]
RUN dotnet restore "./BlastService.Private.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BlastService.Private.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlastService.Private.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="./webserver.pfx"
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=mpdbe000
WORKDIR /app
COPY --from=publish /app/publish .
COPY webserver.pfx .

ENTRYPOINT ["dotnet", "BlastService.Private.dll"]