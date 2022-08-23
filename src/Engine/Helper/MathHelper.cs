using System.Numerics;
using Raylib_cs;

public static class MathHelper
{
    public static float MoveTowards(float current, float target, float maxDelta)
    {
        if (Math.Abs(target - current) <= maxDelta)
        {
            return target;
        }
        return current + Math.Sign(target - current) * maxDelta;
    }

    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta)
    {
        Vector2 a = target - current;
        float magnitude = a.Length();
        if (magnitude <= maxDistanceDelta || magnitude == 0f)
        {
            return target;
        }
        return current + a / magnitude * maxDistanceDelta;
    }

    public static float RandomRange(float min, float max)
    {
        return (Random.Shared.NextSingle() * (max - min)) + min;
    }

    public static float Lerp(float firstFloat, float secondFloat, float by)
    {
        var byClamp = Math.Clamp(by, 0f, 1f);
        return firstFloat * (1 - by) + secondFloat * byClamp;
    }

    public static int Lerp(int first, int second, float by)
    {
        return (int)Math.Round(Lerp((float)first, (float)second, by));
    }

    public static Vector2 Vector2Lerp(Vector2 initial, Vector2 final, float t)
    {
        return new Vector2(
            Lerp(initial.X, final.X, t),
            Lerp(initial.Y, final.Y, t)
        );
    }

    public static Color ColorLerp(Color initial, Color final, float t)
    {
        return new Color(
            Lerp(initial.r, final.r, t),
            Lerp(initial.g, final.g, t),
            Lerp(initial.b, final.b, t),
            Lerp(initial.a, final.a, t)
        );
    }
}