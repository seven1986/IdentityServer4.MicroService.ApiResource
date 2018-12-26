namespace IdentityServer4.MicroService.ApiResource
{
    public interface IPagingResult
    {
        int take { get; set; }

        int skip { get; set; }

        int code { get; set; }

        int total { get; set; }

        string message { get; set; }
    }
}
