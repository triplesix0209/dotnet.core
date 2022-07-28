using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TripleSix.Core.Helpers;

namespace TripleSix.Core.Identity
{
    public class BaseIdentityContext : IIdentityContext
    {
        internal const string ClaimKeyId = "id";
        internal const string ClaimKeyPermission = "permission";
        internal const string ClaimKeyAccessLevel = "accessLevel";

        /// <summary>
        /// Khởi tạo <see cref="BaseIdentityContext"/>.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        public BaseIdentityContext(HttpContext httpContext)
        {
            var tokenData = GetAccessTokenData(httpContext);
            if (tokenData == null) return;

            var idClaim = tokenData.Claims.FirstOrDefault(x => x.Type == ClaimKeyId);
            if (idClaim != null && !idClaim.Value.IsNullOrWhiteSpace())
                UserId = Guid.Parse(idClaim.Value);

            var accessLevelClaim = tokenData.Claims.FirstOrDefault(x => x.Type == ClaimKeyAccessLevel);
            if (accessLevelClaim != null && !accessLevelClaim.Value.IsNullOrWhiteSpace())
                AccessLevel = int.Parse(accessLevelClaim.Value);

            var permissionClaim = tokenData.Claims.FirstOrDefault(x => x.Type == ClaimKeyPermission);
            if (permissionClaim != null)
                Permissions = permissionClaim.Value.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Id tài khoản.
        /// </summary>
        public virtual Guid? UserId { get; protected set; }

        /// <summary>
        /// Danh sách quyền.
        /// </summary>
        public virtual string[]? Permissions { get; protected set; }

        public int AccessLevel { get; protected set; }

        /// <summary>
        /// Đã xác thực hay chưa.
        /// </summary>
        public virtual bool IsAuthorized => UserId.HasValue;

        /// <inheritdoc/>
        public virtual List<Claim> GenerateClaim<T>(
            T data,
            int accessLevel,
            Expression<Func<T, Guid>>? idSelector = null,
            IEnumerable<string>? permissions = null,
            Func<T, List<Claim>>? customGenerator = null)
        {
            var result = new List<Claim>();

            var propertyIdName = (idSelector?.Body as MemberExpression)?.Member.Name;
            if (propertyIdName.IsNullOrWhiteSpace()) propertyIdName = "Id";
            var id = typeof(T).GetProperty(propertyIdName)?.GetValue(data)?.ToString();
            if (id != null)
                result.Add(new Claim(ClaimKeyId, id, ClaimValueTypes.String));

            result.Add(new Claim(ClaimKeyAccessLevel, accessLevel.ToString(), ClaimValueTypes.Integer));

            if (!permissions.IsNullOrEmpty())
            {
                result.Add(new Claim(
                    ClaimKeyPermission,
                    permissions.ToString(" "),
                    ClaimValueTypes.String));
            }

            if (customGenerator == null)
            {
                var properties = typeof(T).GetProperties()
                .Where(x => x.Name != propertyIdName);
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(data);
                    if (propertyValue == null) continue;

                    string? claimValue;
                    if (propertyValue is DateTime datetime)
                        claimValue = datetime.ToEpochTimestamp().ToString();
                    else
                        claimValue = propertyValue.ToString();
                    if (claimValue == null) continue;

                    result.Add(new Claim(property.Name.ToCamelCase(), claimValue, ClaimValueTypes.String));
                }
            }
            else
            {
                var customClaims = customGenerator(data);
                result.AddRange(customClaims);
            }

            return result;
        }

        /// <summary>
        /// Đọc dữ liệu từ access token.
        /// </summary>
        /// <param name="httpContext"><see cref="HttpContext"/>.</param>
        /// <returns><see cref="JwtSecurityToken"/>.</returns>
        protected virtual JwtSecurityToken? GetAccessTokenData(HttpContext httpContext)
        {
            var request = httpContext.Request;
            if (request == null) return null;

            var accessToken = request.Headers.Authorization.ToString().Trim();
            if (accessToken.StartsWith("bearer ", StringComparison.CurrentCultureIgnoreCase))
                accessToken = accessToken[7..];
            if (accessToken.IsNullOrWhiteSpace()) return null;

            return new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;
        }
    }
}
