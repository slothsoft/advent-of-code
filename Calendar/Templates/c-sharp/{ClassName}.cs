using System.Collections.Generic;
using System.Linq;

namespace AoC.day{day};

/// <summary>
/// <a href="https://adventofcode.com/{year}/day/{day}">Day {day}: {Title}</a>
/// </summary>
public class {ClassName} {
    internal record MyClass(string Property) {
    }

    public {ClassName}(IEnumerable<string> input) {
        Input = ParseInput(input);
    }

    internal MyClass[] Input { get; }

    internal static MyClass[] ParseInput(IEnumerable<string> input) {
        return input.Select(s => new MyClass(s)).ToArray();
    }

    public long Calculate() {
        return 7;
    }
}