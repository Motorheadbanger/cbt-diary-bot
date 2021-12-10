using System;
using System.Threading.Tasks;
using Telegram.Bot;
using static cbt_diary_bot.States;
using static cbt_diary_bot.Keyboards;

namespace cbt_diary_bot
{
    public class Methods
    {
        public static async Task<State> Start(ITelegramBotClient bot, long chatId)
        {
            await bot.SendTextMessageAsync(
                chatId,
                "Привет! Нажми на кнопку \"Новая ситуация\", чтобы сделать новую запись в дневник.",
                replyMarkup: startingKeyboard
            );

            return State.Started;
        }

        public static async Task<State> NewIncident(ITelegramBotClient bot, long chatId)
        {
            await bot.SendTextMessageAsync(
                chatId,
                "Привет! Что ты сейчас чувствуешь?",
                replyMarkup: emotionsKeyboard
            );

            return State.EmotionAsked;
        }

        public static async Task<State> EmotionsStrength(ITelegramBotClient bot, long chatId)
        {
            await bot.SendTextMessageAsync(
                chatId,
                "Насколько сильна твоя эмоция (оцени от 1 до 10)?",
                replyMarkup: emotionStrengthKeyboard
            );

            return State.EmotionStrengthAsked;
        }

        public static async Task<State> EmotionsStrengthAfter(ITelegramBotClient bot, long chatId)
        {
            await bot.SendTextMessageAsync(
                chatId,
                "Уменьшилась ли сила твоих эмоций? Оцени силу своих переживаний от 1 до 10.",
                replyMarkup: emotionStrengthKeyboard
            );

            return State.EmotionsStrengthAfterAsked;
        }

        public static async Task<State> End(ITelegramBotClient bot, long chatId)
        {
            await bot.SendTextMessageAsync(
                chatId,
                "Отлично! Ты молодец!",
                replyMarkup: startingKeyboard
            );

            return State.Started;
        }

        public static async Task<State> SendMessage(ITelegramBotClient bot, long chatId, State state)
        {
            await bot.SendTextMessageAsync(
                chatId,
                GetReplyMessage(state),
                replyMarkup: replyKeyboardRemove
            );

            return state.Next();
        }

        public static string GetToken()
        {
            var token = string.Empty;
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && token.Length > 0)
                {
                    token = token[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    token += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return token;
        }

        private static string GetReplyMessage(State state)
        {
            return state switch
            {
                State.Empty => "Привет! Нажми на кнопку \"Новая ситуация\", чтобы сделать новую запись в дневник.",
                State.Started => "Привет! Что ты сейчас чувствуешь?",
                State.EmotionAsked => "Насколько сильна твоя эмоция (оцени от 1 до 10)?",
                State.EmotionStrengthAsked => "Что случилось? Из-за какого события ты себя так чувствуешь?",
                State.CircumstancesAsked => "Какие мысли возникли в твоей голове в этот момент? Услышь всю цепочку мыслей, и найди основную мысль, которая вызвала у тебя эмоции.",
                State.ThoughtsAsked => "Какие есть доводы \"ЗА\" и \"ПРОТИВ\" этого утверждения? Приводи только реальные факты, а не общие понятия или личные оценки.",
                State.WeighingAsked => "Возможны ли другие объяснения ситуации?",
                State.AlternativesAsked => "Свойственно ли людям совершать подобные поступки, как тот, что совершил ты или по отношению к тебе?\nЧто бы в этой ситуации подумал какой-то другой человек(твой друг, коллега)? Что бы ты сказал(а) другу в подобной ситуации?",
                State.PerspectiveAsked => "Какие преимущества и недостатки дает такой способ мышления?",
                State.PerspectiveWeighingAsked => "Не допускаешь ли ты логических ошибок в своих действиях? То есть, ты хочешь достичь определенного результата, но твои действия, очевидно, ведут к другому результату.",
                State.LogicErrorsAsked => "Не рассуждаешь ли ты в категориях \"всё или ничего\" (\"идеально или ужасно\", \"герой или неудачник\" и т.д.)?",
                State.AllOrNothingAsked => "Какие действия ты можешь предпринять, чтобы решить эту проблему (предотвратить в будущем)?",
                State.PreventiveActionsAsked => "Что бы ты себе сказал, будь тебе 80 лет?",
                State.MessageToYoungSelfAsked => "Уменьшилась ли сила твоих эмоций? Оцени силу своих переживаний от 1 до 10.",
                State.EmotionsStrengthAfterAsked => "Какой мыслью/установкой ты можешь заменить ту, которая первоначально возникла в твоей голове?",
                State.ReplacementAsked => "Отлично! Ты молодец!",
                _ => null,
            };
        }
    }
}
