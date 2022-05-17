using System.Collections.Concurrent;

namespace ChatSample.Services;

public class GroupNamesService
{
    private BlockingCollection<string> _groupNames = new();

    public IEnumerable<string> GetGroups() => _groupNames;

    public void AddGroup(string group)
    {
        _groupNames.Add(group);
    }
}
