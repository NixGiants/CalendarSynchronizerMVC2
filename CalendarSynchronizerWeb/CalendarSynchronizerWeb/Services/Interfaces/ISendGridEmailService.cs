namespace CalendarSynchronizerWeb.Services.Interfaces
{
    public interface ISendGridEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
