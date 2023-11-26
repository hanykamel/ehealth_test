FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# copy all the layers' csproj files into respective folders
COPY ["./EHealth.ManageItemLists.Application/EHealth.ManageItemLists.Application.csproj", "src/EHealth.ManageItemLists.Application/"]
COPY ["./EHealth.ManageItemLists.DataAccess/EHealth.ManageItemLists.DataAccess.csproj", "src/EHealth.ManageItemLists.DataAccess/"]
COPY ["./EHealth.ManageItemLists.Domain/EHealth.ManageItemLists.Domain.csproj", "src/EHealth.ManageItemLists.Domain/"]
COPY ["./EHealth.ManageItemLists.Infrastructure/EHealth.ManageItemLists.Infrastructure.csproj", "src/EHealth.ManageItemLists.Infrastructure/"]
COPY ["./EHealth.ManageItemLists.Presentation/EHealth.ManageItemLists.Presentation.csproj", "src/EHealth.ManageItemLists.Presentation/"]

# run restore over project - this pulls restore over the dependent projects as well
RUN dotnet restore "src/EHealth.ManageItemLists.Presentation/EHealth.ManageItemLists.Presentation.csproj"

COPY . .

# run build over the project
WORKDIR "/src/EHealth.ManageItemLists.Presentation/"
RUN dotnet build -c Release -o /app/build

# run publish over the API project
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS runtime
WORKDIR /app

COPY --from=publish /app/publish .
RUN ls -l
ENTRYPOINT [ "dotnet", "EHealth.ManageItemLists.Presentation.dll" ]
