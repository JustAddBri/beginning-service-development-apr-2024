namespace HelpDeskSupportApi.Services;

public interface IProvideTheBusinessClock
{
    Task<bool> AreWeCurrentOpenAsync();
}
