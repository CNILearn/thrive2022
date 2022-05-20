
RepeatSequence str = new('A');

for (int i = 0; i < 10; i++)
{
    Console.WriteLine(str++);
}
Console.WriteLine();

Vector<double> v1 = new(1, 2, 3);
Vector<double> v2 = new(4, 5, 6);
Vector<double> v3 = v1 + v2;
Console.WriteLine(v3);

public struct RepeatSequence : IGetNext<RepeatSequence>
{
    public RepeatSequence(char init)
    {
        ch = init;
        Text = new string(ch, 1);
    }

    private readonly char ch;
    public string Text { get; private set; }
    public static RepeatSequence operator ++(RepeatSequence other) =>
        other with { Text = other.Text + other.ch };

    public override string ToString() => Text;
}

public interface IGetNext<T>
    where T : IGetNext<T>
{
    static abstract T operator ++(T other);
}

public record struct Vector<T>(T X, T Y, T Z) : IAdditionOperators<Vector<T>, Vector<T>, Vector<T>>
    where T : IAdditionOperators<T, T, T>, ISubtractionOperators<T, T, T>
{
    public static Vector<T> operator +(Vector<T> left, Vector<T> right)
    {
        return new Vector<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }
}
