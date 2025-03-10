﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomeworkBot.Configuration;
using HomeworkBot.Controllers;
using HomeworkBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HomeworkBot.Controllers;
using Telegram.Bot;

namespace HomeworkBot;

public class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        // Объект, отвечающий за постоянный жизненный цикл приложения
        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
            .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
            .Build(); // Собираем

        Console.WriteLine("Сервис запущен");
        // Запускаем сервис
        await host.RunAsync();
        Console.WriteLine("Сервис остановлен");
    }

    static void ConfigureServices(IServiceCollection services)
    {

        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(appSettings);

        services.AddSingleton<IStorage, MemoryStorage>();

        services.AddTransient<TextMessageController>();

        services.AddTransient<DefaultMessageController>();

        services.AddTransient<InlineKeyboardController>();

        // Регистрируем объект TelegramBotClient c токеном подключения
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("7618015442:AAEtarOCPeR4U8nE81NAu-0n7Ss2p36kLp4"));
        // Регистрируем постоянно активный сервис бота
        services.AddHostedService<Bot>();
    }

    static AppSettings BuildAppSettings()
    {
        return new AppSettings()
        {
            BotToken = "7618015442:AAEtarOCPeR4U8nE81NAu-0n7Ss2p36kLp4",
        };
    }

}
