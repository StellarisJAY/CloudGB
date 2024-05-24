namespace CloudGB.Web.WebRTC
{
    public interface INegotiationHub
    {
        Task SendAsync(string message);
    }
}
