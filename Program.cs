using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static cbt_diary_bot.States;
using static cbt_diary_bot.Methods;
using File = System.IO.File;

var bot = new TelegramBotClient(GetToken());

using var cts = new CancellationTokenSource();

bot.StartReceiving(
    new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
    cts.Token);

Console.ReadLine();
cts.Cancel();

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
{
    if (update.Type != UpdateType.Message)
        return;
    if (update.Message.Type != MessageType.Text)
        return;

    var chat = update.Message.Chat.Id;
    string path = $"{chat}.xml";

    State state = await GetState(path);

    switch (state)
    {
        case State.Empty:
            if (update.Message.Text == "/start")
                state = await Start(bot, chat);
                SetState(path, state);
            break;

        case State.Started:
            if (update.Message.Text == "Новая ситуация")
                state = await NewIncident(bot, chat);
                SetState(path, state);
            break;

        case State.EmotionAsked:
            state = await EmotionsStrength(bot, chat);
                SetState(path, state);
            break;


        case State.MessageToYoungSelfAsked:
            state = await EmotionsStrengthAfter(bot, chat);
                SetState(path, state);
            break;

        case State.ReplacementAsked:
            state = await End(bot, chat);
                SetState(path, state);
            break;

        default:
            state = await SendMessage(bot, chat, state);
                SetState(path, state);
            break;
    }
}

async Task<State> GetState(string path)
{
    State state = State.Empty;

    if (!File.Exists(path))
    {
        await File.WriteAllLinesAsync(path, new[] { "0" });
    }

    state = (State) int.Parse((await File.ReadAllLinesAsync(path))[0]);

    return state;
}

async void SetState(string path, State state)
{
    await File.WriteAllLinesAsync(path, new[] { ((int)state).ToString() });
}