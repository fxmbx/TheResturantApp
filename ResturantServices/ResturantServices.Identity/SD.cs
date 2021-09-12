using Duende.IdentityServer;
using Duende.IdentityServer.Models;
namespace ResturantServices.Identity
{
    public class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                    new ApiScope("resturant", "Resturant Server"),
                    new ApiScope(name: "read", displayName : "Read your data"),
                    new ApiScope(name: "write", displayName : "Write your data"),
                    new ApiScope(name: "delete", displayName : "Delete your data")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                    new Client //generic client
                    {
                        ClientId  = "client",
                        ClientSecrets  = {new Secret("secret".Sha256())},
                        AllowedGrantTypes   = GrantTypes.ClientCredentials,
                        AllowedScopes = {"read", "write", "profile"} //profile is a built inn scope
                    },
                    new Client
                    {
                        ClientId  = "resturant",
                        ClientSecrets  = {new Secret("secret".Sha256())},
                        AllowedGrantTypes = GrantTypes.Code,
                        RedirectUris =  {"https://localhost:44387/signin-oidc"}, //get the url from the FE launch setting and replace the number part with the sslport numbers and add he signin-oidc
                        PostLogoutRedirectUris = {"https://localhost:44387/siginout-callback-oidc"},
                        AllowedScopes = new List<string>
                        {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.Profile,
                           IdentityServerConstants.StandardScopes.Email,
                           "resturant"
                        },

                    //    RequirePkce = true,
                    //     AllowPlainTextPkce = false 
                    },
            };
    }
}