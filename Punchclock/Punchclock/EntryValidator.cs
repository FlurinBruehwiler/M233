using Punchclock.Models;

namespace Punchclock;

public class EntryValidator
{
    public bool IsValid(Entry entry)
    {
        return entry.CheckIn < entry.CheckOut;
    }
}