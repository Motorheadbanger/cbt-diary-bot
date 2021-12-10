using System;

namespace cbt_diary_bot
{
    public static class States
    {
        public enum State
        {
            Empty,
            Started,
            EmotionAsked,
            EmotionStrengthAsked,
            CircumstancesAsked,
            ThoughtsAsked,
            WeighingAsked,
            AlternativesAsked,
            PerspectiveAsked,
            PerspectiveWeighingAsked,
            LogicErrorsAsked,
            AllOrNothingAsked,
            PreventiveActionsAsked,
            MessageToYoungSelfAsked,
            EmotionsStrengthAfterAsked,
            ReplacementAsked
        }

        public static State Next<State>(this State src) where State : struct
        {
            State[] Arr = (State[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf(Arr, src) + 1;

            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }
}