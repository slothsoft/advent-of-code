using System.Collections.Generic;
using System.Linq;

namespace AoC._07;

/// <summary>
/// My great file system implementation. 
/// </summary>
public class FileSystem {
    public ILocation Root { get; } = new Directory("");
    public Directory CurrentFolder { get; private set; }

    public FileSystem() {
        CurrentFolder = (Directory) Root;
    }

    public void ChooseDirectory(string newDirectory) {
        CurrentFolder = newDirectory == ".."
            // go up
            ? CurrentFolder.Parent!
            // go down
            : CurrentFolder.Children.Where(c => c.Name == newDirectory).OfType<Directory>().Single();
    }
}

public interface ILocation {
    public string Name { get; }
    public int Size { get; }
    public bool IsDirectory { get; }

    IEnumerable<ILocation> Traverse();
}

public record Directory(string Name) : ILocation {
    public int Size => Children.Select(c => c.Size).Sum();
    public bool IsDirectory => true;
    public IEnumerable<ILocation> Children => _children;
    public Directory? Parent { get; private set; }

    private readonly List<ILocation> _children = new();

    public IEnumerable<ILocation> Traverse() {
        IEnumerable<ILocation> result = Enumerable.Repeat(this, 1);
        return Children.Aggregate(result, (current, child) => current.Concat(child.Traverse()));
    }

    public void AddChildren(IEnumerable<ILocation> newChildren) {
        var newChildrenAsArray = newChildren.ToArray();
        foreach (var directory in newChildrenAsArray.OfType<Directory>()) {
            directory.Parent = this;
        }
        _children.AddRange(newChildrenAsArray);
    }
}

public record File(string Name, int Size) : ILocation {
    public bool IsDirectory => false;

    public IEnumerable<ILocation> Traverse() {
        yield return this;
    }
}