using static TrafficLight;

DeclarationConstAndVarPattern();

PropertyAndDiscardPatternSample();

await PositionalPatternSampleAsync();

TypePatternSample();

LogicalPatternSample();

MultiplePatternsSample(); // with relational

ListPatternSample();

void DeclarationConstAndVarPattern()
{
    static void PatternMatchingV1(object o)
    {
        if (o is null)  // const pattern
        {
            throw new ArgumentNullException(nameof(o));
        }

        switch (o)
        {
            case 42:  // const pattern
                Console.WriteLine("42");
                break;
            case string s:  // declaration pattern
                Console.WriteLine("it's a string with the value {s}");
                break;
            case var v1:  // var pattern
                Console.WriteLine("something else: {v1}");
                break;
        }
    }

    Console.WriteLine("Declaration, const, and var patterns");
    PatternMatchingV1(42);
    PatternMatchingV1("42");
    Console.WriteLine();
}

void PropertyAndDiscardPatternSample()
{
    static string Check(Person person) =>
        person switch
        {
            { FirstName: "Clark" } => $"{person} is a Clark",
            { Address: { City: "Smallville" } } => $"{person} is from Smallville",
            { Address.City: "Gotham City" } => $"{person} is from Gotham City",  // Extended property pattern (10.0)
            _ => $"{person} is not listed"  // discard pattern
        };

    Console.WriteLine("Property pattern and discard pattern");
    foreach (var p in GetPeople())
    {
        Console.WriteLine(Check(p));
    }
    Console.WriteLine();
}

async Task PositionalPatternSampleAsync()
{
    // positional pattern, can be used with Deconstruct

    static (TrafficLight Current, TrafficLight Previous) NextLight(TrafficLight current, TrafficLight previous) =>
        (current, previous) switch
        {
            (Red, _) => (Amber, current),
            (Amber, Red) => (Green, current),
            (Green, _) => (Amber, current),
            (Amber, Green) => (Red, current),
            _ => (Amber, current)
        };

    Console.WriteLine("Positional pattern");
    var currentLight = Red;
    var previousLight = Red;
    for (int i = 0; i < 10; i++)
    {
        (currentLight, previousLight) = NextLight(currentLight, previousLight);
        Console.WriteLine($"current light: {currentLight}");
        await Task.Delay(1000);
    }
    Console.WriteLine();
}

void TypePatternSample()
{
    static string CheckType(Shape shape) =>
        shape switch
        {
            Rectangle => "it's a rectangle",
            Ellipse => "it's an ellipse",
            _ => "it's another type deriving from Shape"
        };

    Console.WriteLine("type pattern");
    string result = CheckType(new Ellipse(new(20, 20), new(4, 4)));
    Console.WriteLine(result);
    result = CheckType(new Rectangle(new(20, 20), new(4, 4)));
    Console.WriteLine(result);
    Console.WriteLine();
}

void LogicalPatternSample()
{
    string LogicalOperators(Person p) =>
        p switch
        {
            { PhoneNumber: not null } and { PhoneNumber: not "" } => $"{p.FirstName} {p.LastName}, ({p.PhoneNumber})",
            _ => $"{p.FirstName} {p.LastName}"
        };

    Console.WriteLine("Logical pattern");
    Person p1 = new("Tom", "Turbo");
    Person p2 = new("Bruce", "Wayne", "+108154711");
    Console.WriteLine(LogicalOperators(p1));
    Console.WriteLine(LogicalOperators(p2));
    Console.WriteLine();
}

void MultiplePatternsSample()
{
    static string MultiplePatterns(object? o) =>
        o switch
        {
            null => "just null", // const pattern
            Book b => $"a book with this title: {b.Title}",  // declaration pattern
            42 => "a constant with value 42",  // const pattern
            string[] { Length: > 3 } => "a string array with more than 3 elements",  // property and relational pattern
            string[] => "any other string array",  // type pattern
            _ => "anything else"  // discard
        };

    Console.WriteLine("multiple patterns including relational pattern");
    Console.WriteLine(MultiplePatterns(null));
    Console.WriteLine(MultiplePatterns(new Book("Professional C#", "Wiley")));
    Console.WriteLine(MultiplePatterns(42));
    Console.WriteLine(MultiplePatterns(new [] {"one", "two", "three", "four"}));
    Console.WriteLine(MultiplePatterns(new[] {"one", "two"}));
    Console.WriteLine(MultiplePatterns("more"));
    Console.WriteLine();
}

void ListPatternSample()
{
    static string ListPattern(int[] list) =>
        list switch
        {
            [1, 2, 3] => "1, 2, and 3 in the list",
            [1, 2, .. var x, 5] => $"slice pattern, the values between 1,2 and 5 summarize to: {x.Sum()}",
            { Length: > 3 } => "array with more than three elements",            
            _ => "no match"
        };

    Console.WriteLine("list patterns");
    int[] list1 = { 1, 2, 3 };
    int[] list2 = { 1, 2, 4, 5, 6, 9, 11, 5 };
    int[] list3 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    Console.WriteLine(ListPattern(list1));
    Console.WriteLine(ListPattern(list2));
    Console.WriteLine(ListPattern(list3));
    Console.WriteLine();
}


IEnumerable<Person> GetPeople() =>
    new[] 
    {
        new Person("Clark", "Kent", Address: new Address("Smallville", "USA")),
        new Person("Lois", "Lane", Address: new Address("Smallville", "USA")),
        new Person("Bruce", "Wayne", Address: new Address("Gotham City", "USA")),
        new Person("Alfred", "Pennyworth", Address: new Address("Gotham City", "USA")),
        new Person("Dick", "Grayson", Address: new Address("Gotham City", "USA")),
        new Person("Barry", "Allen", Address: new Address("Central City", "USA")),
    };

public enum TrafficLight
{
    Red,
    Amber,
    Green
}

public record Book(string Title, string Publisher);

public record Address(string City, string Country);

public record Person(string FirstName, string LastName, string? PhoneNumber = null, Address? Address = null)
{
    public override string ToString() => $"{FirstName} {LastName}";
}

public readonly record struct Position(double X, double Y);
public readonly record struct Size(double Height, double Width);
public abstract record class Shape(Position Position, Size Size);
public record class Ellipse(Position Position, Size Size) : Shape(Position, Size);
public record class Rectangle(Position Position, Size Size) : Shape(Position, Size);
