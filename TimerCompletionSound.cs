namespace TurboClock;

internal static class TimerCompletionSound
{
    public static void Play()
    {
        _ = Task.Run(() => Console.Beep(880, 500));
    }
}