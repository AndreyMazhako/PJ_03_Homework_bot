using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using HomeworkBot.Configuration;

namespace HomeworkBot.Controllers;

public class TextMessageController
{
    private readonly ITelegramBotClient _telegramClient;

    public TextMessageController(ITelegramBotClient telegramBotClient)
    {
        _telegramClient = telegramBotClient;
    }

    public async Task Handle(Message message, CancellationToken ct)
    {
        switch (message.Text)
        {
            case "/start":

                // Объект, представляющий кнопки
                var buttons = new List<InlineKeyboardButton[]>();
                {
                    buttons.Add(new[]
               {
                        InlineKeyboardButton.WithCallbackData($" Количество символов" , $"quantity"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"sum"),

                    });
                };

                // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                await _telegramClient.SendMessage(message.Chat.Id, $"<b>  Наш бот подсчитывает количество символов в тексте.</b> {Environment.NewLine}" +
                    $"{Environment.NewLine}А также считает сумму чисел введенных через пробел.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                break;
            default:
                await ProcessMessage(message, ct);
                break;
        }
    }

    public async Task ProcessMessage(Message message, CancellationToken ct)
    {
        string messageText = message.Text;

        if (messageText.All(char.IsDigit) || (messageText.Any(char.IsWhiteSpace) && messageText.Any(char.IsDigit)))
        {
            try
            {
                long sum = messageText.Split(' ')
                    .Where(s => long.TryParse(s, out long num))
                    .Sum(s => long.Parse(s));

                await _telegramClient.SendMessage(message.Chat.Id, $"Сумма чисел: {sum}", cancellationToken: ct);
            }
            catch (Exception ex)
            {
                await _telegramClient.SendMessage(message.Chat.Id, $"Ошибка при вычислении суммы: {ex.Message}", cancellationToken: ct);
            }
        }
        else
        {
            await _telegramClient.SendMessage(message.Chat.Id, $"Длина сообщения: {messageText.Length} знаков", cancellationToken: ct);
        }
    }
}
