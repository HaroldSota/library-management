using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Model.UserModel;
using LibraryManagement.Core.Configuration;
using LibraryManagement.Core.Domain.Model;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.Application.Handlers.UserHandler
{
    /// <summary>
    ///  User authentication handler
    /// </summary>
    public sealed class AuthenticateHandler : IRequestHandler<AuthenticateRequest, MessageResponse<AuthenticateResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILibraryManagementConfig _appConfig;
        private List<string> Errors { get; } = new List<string>();

        /// <summary>
        ///     AuthenticateHandler ctor.
        /// </summary>
        /// <param name="userRepository">User Repository</param>
        /// <param name="appConfig">App. Config</param>
        public AuthenticateHandler(IUserRepository userRepository, ILibraryManagementConfig appConfig)
        {
            _userRepository = userRepository;
            _appConfig = appConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MessageResponse<AuthenticateResponse>> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if(!Validate(request))
                    return new MessageResponse<AuthenticateResponse>(MessageType.Validation, new MessageResponseError("Validate", Errors.First()));

                var user = await _userRepository.GetByUsername(request.Username);

                // check if username exists
                if (user == null)
                    return new MessageResponse<AuthenticateResponse>(MessageType.NotFound, new MessageResponseError("NotFound", "User not found!"));

                // check if password is correct
                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                    return new MessageResponse<AuthenticateResponse>(MessageType.Validation, new MessageResponseError("Validate", "Username and/or password not correct!"));

                // authentication successful
                return new MessageResponse<AuthenticateResponse>(new AuthenticateResponse()
                {
                    Id =  user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = GetAuthenticationToken(user)
                });
            }
            catch (Exception e)
            {
               return new MessageResponse<AuthenticateResponse>(MessageType.Error ,e);
            }
        }

        private bool Validate(AuthenticateRequest request)
        {
   
            if (string.IsNullOrEmpty(request.Username))
                Errors.Add("Username can not be empty");
            else
            if (string.IsNullOrEmpty(request.Password))
                Errors.Add("Password can not be empty");
            return Errors.Count == 0;
        }
        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        private string GetAuthenticationToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appConfig.AppSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}