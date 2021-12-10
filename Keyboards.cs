using Telegram.Bot.Types.ReplyMarkups;

namespace cbt_diary_bot
{
    public class Keyboards
    {
        public static readonly ReplyKeyboardMarkup startingKeyboard = new(
                new KeyboardButton[] { "Новая ситуация" },
                true,
                true
            );

        public static readonly ReplyKeyboardMarkup emotionsKeyboard = new(
                new[]
                {
                    new KeyboardButton[] { Emotions.Apathy, Emotions.Guilt },
                    new KeyboardButton[] { Emotions.Indignation, Emotions.Anger },
                    new KeyboardButton[] { Emotions.Sadness, Emotions.Discontent },
                    new KeyboardButton[] { Emotions.Resentment, Emotions.Disgust },
                    new KeyboardButton[] { Emotions.Panic, Emotions.Contempt },
                    new KeyboardButton[] { Emotions.Irritation, Emotions.Suffering },
                    new KeyboardButton[] { Emotions.Fear, Emotions.Shame },
                    new KeyboardButton[] { Emotions.Anguish, Emotions.Anxiety },
                    new KeyboardButton[] { Emotions.Gloom, Emotions.Rage }
                },
                true,
                true
            );

        public static readonly ReplyKeyboardMarkup emotionStrengthKeyboard = new(
                new[] {
                    new KeyboardButton[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10"}
                },
                true,
                true
            );

        public static readonly ReplyKeyboardRemove replyKeyboardRemove = new();
    }
}
