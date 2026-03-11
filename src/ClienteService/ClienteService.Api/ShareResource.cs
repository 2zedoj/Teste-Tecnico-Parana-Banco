using Microsoft.Extensions.Localization;

namespace ClienteService.Api
{
    public class ShareResource { }

    public interface IResourceLocalizer
    {
        string Localize(string key);
    }

    public class ResourceLocalizer : IResourceLocalizer
    {
        private readonly IStringLocalizer<ShareResource> _localizer;

        public ResourceLocalizer(IStringLocalizer<ShareResource> localizer)
        {
            _localizer = localizer;
        }

        public string Localize(string key)
        {
            return _localizer[key];
        }
    }
}
