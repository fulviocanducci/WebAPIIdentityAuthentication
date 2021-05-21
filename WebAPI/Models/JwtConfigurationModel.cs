using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Models
{
    public sealed class JwtConfigurationModel
    {
        public JwtConfigurationModel(string validAudience, string validIssuer, string secret)
        {
            ValidAudience = validAudience;
            ValidIssuer = validIssuer;
            Secret = secret;
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
        }
        public string ValidAudience { get; }
        public string ValidIssuer { get; }
        public string Secret { get; }
        public SecurityKey IssuerSigningKey { get; }
    }

}
