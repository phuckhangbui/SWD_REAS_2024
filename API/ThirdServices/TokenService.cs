﻿using API.Entity;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
        }

        public string CreateToken(Account account)
        {
            var claim = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, account.AccountId.ToString()),
            new Claim("RoleId", account.RoleId.ToString()),
            new Claim("AccountId", account.AccountId.ToString())
        };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDiscriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
