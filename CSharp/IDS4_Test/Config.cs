using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDS4_Test
{
    public static class Config
    {
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope(ImageMan, "图像管理系统")
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId="client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets={new Secret("secret".Sha256()) },

                    AllowedScopes={ ImageMan }
                },
            };
        }

        /// <summary>
        /// scope
        /// </summary>
        public static string ImageMan = "imageman";
    }
}
