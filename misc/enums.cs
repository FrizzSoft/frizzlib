namespace FrizzLib.Misc;

/// <summary>
/// An enumeration of the months of the year in short form (eg. "Jan" - "Dec").
/// </summary>
public enum MonthsOfYear_Short { Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec }
/// <summary>
/// An enumeration of the months of the year in long form (eg. "January" - "December").
/// </summary>
public enum MonthsOfYear_Long
{
    January, February, March, April, May, June, July, August, September, October, November, December
}
/// <summary>
/// An enumeration of the days of the week in short form, starting from Monday (eg. "Mon" - "Sun").
/// </summary>
public enum DaysOfWeek_Short { Mon, Tue, Wed, Thu, Fri, Sat, Sun }
/// <summary>
/// An enumeration of the days of the week in long form, starting from Monday (eg. "Monday" - "Sunday").
/// </summary>
public enum DayOfWeek_Long : byte
{
    Monday = 0x01,
    Tuesday = 0x02,
    Wednesday = 0x04,
    Thursday = 0x08,
    Friday = 0x10,
    Saturday = 0x20,
    Sunday = 0x40
};