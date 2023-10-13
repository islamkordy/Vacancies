using Application.Features.User.DTO;
using Application.Features.User.TokenService.Abstract;
using Application.Utils;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Persistence.IRepositories;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Users.Login
{
    internal class LoginHandler: IRequestHandler<LoginCommand, ResponseModel<LoginDto>>
    {
        private readonly IValidator<LoginCommand> _phoneNumberValidator;
        private readonly IGenericRepository<UserAccount> _userAccountRepo;
        private readonly ITokenService _tokenService;

        public LoginHandler(IValidator<LoginCommand> phoneNumberValidator, IGenericRepository<UserAccount> userAccountRepo, ITokenService tokenServic)
        {
            _phoneNumberValidator = phoneNumberValidator;
            _userAccountRepo = userAccountRepo;
            _tokenService = tokenServic;
        }

        public async Task<ResponseModel<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var resultValidation = await _phoneNumberValidator.ValidateAsync(request);
            if (!resultValidation.IsValid)
            {
                return new ResponseModel<LoginDto> { Ok = false, Message = Helpers.ArrangeValidationErrors(resultValidation.Errors) };
            }

            var userAccount = await _userAccountRepo.GetObj(x => x.MobileNumber == request.MobileNumber && x.RoleId == (int)request.RoleId);
            if (userAccount == null)
            {
                return new ResponseModel<LoginDto> { Ok = false, Message = "" };
            }

            if (!VerifyPassword(request.Password, userAccount.PwdSalt!, userAccount.PwdHash!))
            {
                return new ResponseModel<LoginDto> { Ok = false, Message = "InvalidPassword" };
            }

            return await MappedUsers(userAccount);
        }

        private async Task<ResponseModel<LoginDto>> MappedUsers(UserAccount userAccount)
        {
            var token = (await _tokenService.GenerateTokens(userAccount));

            var userAccountIndividualMapped = new LoginDto() {
                MobileNumber = userAccount.MobileNumber,
                Email = userAccount.Email,
                FullName = userAccount.FullName,
                AccessToken = token.accessToken,
                RefreshToken = token.refreshToken,
                RoleId = userAccount.RoleId
            };
            return new ResponseModel<LoginDto> { Ok = true, Message = "Success", Data = userAccountIndividualMapped };

        }

        public bool VerifyPassword(string password, string PwdSalt, string storedSaltedHash)
        {
            byte[] saltBytes = Convert.FromBase64String(PwdSalt);

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                byte[] saltedHashBytes = new byte[saltBytes.Length + hashBytes.Length];

                Buffer.BlockCopy(saltBytes, 0, saltedHashBytes, 0, saltBytes.Length);
                Buffer.BlockCopy(hashBytes, 0, saltedHashBytes, saltBytes.Length, hashBytes.Length);

                string saltedHash = Convert.ToBase64String(saltedHashBytes);

                return saltedHash == storedSaltedHash;
            }
        }
    }
}

