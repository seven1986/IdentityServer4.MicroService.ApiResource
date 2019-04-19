using System;
using System.Collections.Generic;

namespace IdentityServer4.MicroService.ApiResource
{
   public class MicroserviceConfig
    {
        public Uri IdentityServer { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public List<string> ClientIDs { get; set; } = new List<string>();

        public List<string> RedirectUrls { get; set; } = new List<string>();
    }
}
