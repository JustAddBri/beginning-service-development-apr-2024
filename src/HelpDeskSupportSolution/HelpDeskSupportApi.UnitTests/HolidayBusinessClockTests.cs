using HelpDeskSupportApi.Services;
using Microsoft.Extensions.Time.Testing;

namespace HelpDeskSupportApi.UnitTests;
public class HolidayBusinessClockTests
{

    [Theory]
    [MemberData(nameof(OpenTimeExamples))]
    public async Task Open(DateTimeOffset dateToUse)
    {
        var fakeTime = new FakeTimeProvider(startDateTime: dateToUse);

        var sut = new HolidaysBusinessClock(fakeTime);


        Assert.True(await sut.AreWeCurrentOpenAsync());
    }

    [Theory]
    [MemberData(nameof(ClosedOnWeekdaysExamples))]
    public async Task ClosedDuringTheWeek(DateTimeOffset dateToUse)
    {
        var fakeTime = new FakeTimeProvider(startDateTime: dateToUse);

        var sut = new HolidaysBusinessClock(fakeTime);

        Assert.False(await sut.AreWeCurrentOpenAsync());
    }

    [Theory]
    [MemberData(nameof(ClosedOnWeekendsExamples))]
    public async Task ClosedOnWeekends(DateTimeOffset dateToUse)
    {
        var fakeTime = new FakeTimeProvider(startDateTime: dateToUse);

        var sut = new HolidaysBusinessClock(fakeTime);

        Assert.False(await sut.AreWeCurrentOpenAsync());
    }

    [Theory]
    [MemberData(nameof(ClosedOnHolidaysExamples))]
    public async Task ClosedOnHolidays(DateTimeOffset dateToUse)
    {
        var fakeTime = new FakeTimeProvider(startDateTime: dateToUse);

        var sut = new HolidaysBusinessClock(fakeTime);

        Assert.False(await sut.AreWeCurrentOpenAsync());
    }

    // We are closed on monday and friday outside of those hours

    // we are closed on the weekends.

    // on the backlog, we are closed on certain holidays...


    public static IEnumerable<object[]> OpenTimeExamples => [
            [ new DateTimeOffset(2024, 04,23, 9,0,0, new TimeSpan(-4,0,0))], // right at 8 am on a weekend
        [new DateTimeOffset(2024, 04,23,16,59,59,59, new TimeSpan(-4,0,0))] // right at 4:59:59 on a weekday
        ];

    public static IEnumerable<object[]> ClosedOnWeekdaysExamples = [
        [new DateTimeOffset(2024 ,04,23,8,59,59,59, new TimeSpan(-4,0,0))],
        [new DateTimeOffset(2024, 04,23,17,0,0,0, new TimeSpan(-4,0,0))]
        ];

    public static IEnumerable<object[]> ClosedOnWeekendsExamples = [
        [new DateTimeOffset(2024,04,20, 10, 15,00,00, new TimeSpan(-4,0,0))]
        // etc. etc. as many as you need to be confident.
        ];

    public static IEnumerable<object[]> ClosedOnHolidaysExamples => [
            [new DateTimeOffset(2024, 12, 25, 13,00,00,00, new TimeSpan(-4,0,0))],
            [new DateTimeOffset(2024, 7, 4, 13,00,00,00, new TimeSpan(-4,0,0))]

        ];
}
