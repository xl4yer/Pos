using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Pos.Class;
using Pos.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pos.Services
{
    public class UserServices
    {
        private readonly AppDb _constring;
        public IConfiguration Configuration;
        private readonly AppSettings _appSetting;

        public UserServices(AppDb constring, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _constring = constring;
            Configuration = configuration;
            _appSetting = appSettings.Value;
        }
        public async Task<List<users>> Login(string username, string password)
        {
            List<users> u = new();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("Userlogin", con)
                {
                    CommandType = CommandType.StoredProcedure,
                };
                com.Parameters.Clear();
                com.Parameters.AddWithValue("_username", username);
                com.Parameters.AddWithValue("_password", password);

                var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                while (await rdr.ReadAsync().ConfigureAwait(false))
                {
                    u.Add(new users
                    {
                        userID = rdr["userID"].ToString(),
                        name = rdr["Name"].ToString(),
                        username = rdr["Username"].ToString(),
                        password = rdr["Password"].ToString(),
                        role = rdr["Role"].ToString(),
                    });
                }
                if (u.Count > 0)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSetting.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                    new Claim (ClaimTypes.Name ,username),
                    new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                        }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new
                        SymmetricSecurityKey(key), SecurityAlgorithms.Aes128CbcHmacSha256)

                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    u[0].token = tokenHandler.WriteToken(token);
                }
                return u;
            }
        }
    }
}
