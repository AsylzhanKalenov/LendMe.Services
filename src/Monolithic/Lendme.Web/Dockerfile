# Dockerfile для разработки с hot reload для .NET 9
FROM mcr.microsoft.com/dotnet/sdk:9.0

# Устанавливаем рабочую директорию
WORKDIR /app

# Открываем порты
EXPOSE 8080
EXPOSE 8081

# Устанавливаем глобальные инструменты
RUN dotnet tool install --global dotnet-ef --version 9.0.0
RUN dotnet tool install --global dotnet-watch --version 9.0.0

# Добавляем инструменты в PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Копируем файл проекта для восстановления зависимостей
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы
COPY . ./

# Настройки для разработки .NET 9
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_HTTPS_PORTS=8081
ENV DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1

# Команда для запуска с hot reload
CMD ["dotnet", "watch", "run", "--no-build", "--urls", "http://0.0.0.0:8080;https://0.0.0.0:8081"]