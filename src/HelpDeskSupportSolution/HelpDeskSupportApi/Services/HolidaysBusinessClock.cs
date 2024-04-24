
namespace HelpDeskSupportApi.Services;

public class HolidaysBusinessClock(TimeProvider timeProvider) : IProvideTheBusinessClock
{
    public Task<bool> AreWeCurrentOpenAsync()
    {
        var now = timeProvider.GetLocalNow();

        var dayOftheWeek = now.DayOfWeek;
        var hour = now.Hour;
        var openingTime = new TimeSpan(9, 0, 0);
        var closingTime = new TimeSpan(17, 0, 0);

        if (now.Day == 25 && now.Month == 12 || now.Day == 4 && now.Month == 7)
        {
            return Task.FromResult(false);
        }
        var isOpen = dayOftheWeek switch
        {
            DayOfWeek.Sunday => false,
            DayOfWeek.Saturday => false,
            _ => hour >= openingTime.Hours && hour < closingTime.Hours
        };
        return Task.FromResult(isOpen);
    }
}
