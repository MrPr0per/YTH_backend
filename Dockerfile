# ---- build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# копируем csproj и восстанавливаем зависимости (кеширование слоёв)
COPY ["YTH_backend/YTH_backend.csproj", "YTH_backend/"]
RUN dotnet restore "YTH_backend/YTH_backend.csproj"

# копируем всё и билдим/publish
COPY . .
RUN dotnet publish "YTH_backend/YTH_backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---- runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# (опционально) задаём переменную чтобы ASP.NET слушал на 80 порте
ENV ASPNETCORE_URLS=http://+:80
# Если хочешь логировать в консоль
ENV DOTNET_RUNNING_IN_CONTAINER=true

# копируем опубликованные файлы
COPY --from=build /app/publish .

# порт, который пробрасываем наружу
EXPOSE 80

# HEALTHCHECK (опционально) — проверяет /health
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl -f http://localhost/health || exit 1

# Запуск приложения — замените YTH_backend.dll на имя вашей сборки при необходимости
ENTRYPOINT ["dotnet", "YTH_backend.dll"]
