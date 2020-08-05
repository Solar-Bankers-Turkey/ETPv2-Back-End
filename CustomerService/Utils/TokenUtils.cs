using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace CustomerService.Utils {
    public class TokenUtils {
        public static string ReadToken(Microsoft.AspNetCore.Http.HttpRequest request, string key) {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = request.Headers["Authorization"];
            if (authHeader == null) {
                return "";
            }
            authHeader = authHeader.Replace("Bearer ", "");
            var jsonToken = handler.ReadToken(authHeader);
            var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;

            var role = tokenS.Claims.First(claim => claim.Type == key).Value;
            return role;
        }
    }
}
