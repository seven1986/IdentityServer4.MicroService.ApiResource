namespace IdentityServer4.MicroService.ApiResource
{
    public interface IApiResult<T>
    {
        int code { get; set; }

        string codeName { get; set; }

        string message { get; set; }

        T data { get; set; }
    }
}
