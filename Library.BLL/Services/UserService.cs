using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Library.BLL.Dto.Extensions;
using Library.BLL.Dto.RequestModel;
using Library.BLL.Dto.ResponseModel;
using Library.BLL.Services.Interfaces;
using Library.DAL.Entities;
using Library.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Library.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IUserTokenRepo _userTokenRepo;

        public UserService(IUserRepo userRepo, IUserTokenRepo userTokenRepo)
        {
            this._userRepo = userRepo;
            this._userTokenRepo = userTokenRepo;
        }

        public async Task<MiscResponse<UserResponseModel>> CreateUserAsync(CreateUserRequestModel model)
        {
            var existingUser = this._userRepo.Find(x => x.Email == model.Email);
            if (existingUser.Any())
                throw new ArgumentException($"A user exists with the given email '{model.Email}'");

            string salt = generateSalt();
            var userId = await this._userRepo.AddAsync(new ApplicationUser
            {
                Email = model.Email,
                IsAdmin = model.IsAdmin,
                Name = model.Name,
                Salt = salt,
                PasswordHash = hashPassword(model.Password, salt)
            });

            return new MiscResponse<UserResponseModel>()
            {
                Message = "User created successfully",
                Data = new UserResponseModel
                {
                    Email = model.Email,
                    Id = userId,
                    IsAdmin = model.IsAdmin,
                    Name = model.Name
                }
            };
        }

        public Task<MiscResponse<object>> DeleteUserAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<MiscResponse<UserResponseModel>> UpdateUserAsync(UpdateUserRequestModel model)
        {
            var existingUser = this._userRepo.Find(x => x.Id == model.Id).FirstOrDefault();
            if (existingUser == null)
                throw new ArgumentException($"A user does not exist with the given id '{model.Id}'");

            if (!string.IsNullOrWhiteSpace(model.Email))
            {
                var existingUserWithEmail = this._userRepo.Find(x => x.Email == model.Email && x.Id != model.Id);
                if (existingUserWithEmail.Any())
                    throw new ArgumentException($"A user already exists with the updated email '{model.Email}'");
                existingUser.Email = model.Email;
            }

            if (string.IsNullOrWhiteSpace(model.Name))
                existingUser.Name = model.Name;

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                existingUser.Salt = generateSalt();
                existingUser.PasswordHash = hashPassword(model.Password, existingUser.Salt);

                var userTokens = this._userTokenRepo.Find(x => x.UserId == model.Id).ToList();
                var updates = userTokens.Select(x => this._userTokenRepo.DeleteAsync(x)).ToArray();
                Task.WaitAll(updates);
            }

            if (model.IsAdmin.HasValue)
                existingUser.IsAdmin = model.IsAdmin.Value;

            await this._userRepo.UpdateAsync(existingUser);
            return new MiscResponse<UserResponseModel> {
                Data = existingUser.ToModel(),
                Message = "User updated successfully"
            };
        }

        public SearchResponse<UserResponseModel> SearchForUser(SearchUserRequestModel model, int page, int size)
        {
            var allUsers = this._userRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(model.Email))
                allUsers = allUsers.Where(x => x.Email.ToLower().Contains(model.Email.Trim().ToLower()));

            if (!string.IsNullOrWhiteSpace(model.Name))
                allUsers = allUsers.Where(x => x.Name.ToLower().Contains(model.Name.Trim().ToLower()));

            if (model.IsAdmin.HasValue)
                allUsers = allUsers.Where(x => x.IsAdmin == model.IsAdmin.Value);

            return new SearchResponse<UserResponseModel> {
                Page = page,
                TotalCount = allUsers.Count(),
                Result = allUsers.ToList().Skip((page - 1) * size).Take(size).Select(x => x.ToModel())
            };
        }

        public UserResponseModel GetUserWithCredentials(string email, string password)
        {
            var existinguser = this._userRepo.Find(x => x.Email.ToLower() == email).FirstOrDefault();
            if (existinguser == null)
                throw new ArgumentException($"The given credentials are incorrect");

            string hashedPassword = hashPassword(password, existinguser.Salt);
            if (existinguser.PasswordHash != hashedPassword)
                throw new ArgumentException($"The given credentials are incorrect");

            return existinguser.ToModel();
        }

        public async Task CreateUserToken(string token, long userId)
        {
            await this._userTokenRepo.AddAsync(new UserTokens { IsDeleted = false, Token = token, UserId = userId });
        }

        public async Task InvalidateToken(string token)
        {
            var existingToken = this._userTokenRepo.Find(x => x.Token == token).FirstOrDefault();
            if (existingToken == null)
                throw new ArgumentException("Token does not exist");

            existingToken.IsDeleted = true;
            await this._userTokenRepo.UpdateAsync(existingToken);
        }

        public async Task InvalidateAllToken(string userEmail)
        {
            var user = this._userRepo.Find(x => x.Email == userEmail).FirstOrDefault();
            if (user == null)
                throw new ArgumentException("Unable to invalidate tokens for non-existent user");

            var existingTokens = this._userTokenRepo.Find(x => x.UserId == user.Id);
            var tasks = existingTokens.Select(x => this._userTokenRepo.DeleteAsync(x)).ToArray();
            Task.WaitAll(tasks);
        }

        public bool TokenIsValid(string token)
        {
            var existingToken = this._userTokenRepo.Find(x => x.Token == token && !x.IsDeleted.Value);
            return existingToken.Any();
        }

        private string generateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var generator = RandomNumberGenerator.Create()) {
                generator.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        private string hashPassword(string password, string salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                Encoding.UTF8.GetBytes(salt),
                KeyDerivationPrf.HMACSHA1,
                10000,
                256 / 8));
        }
    }
}
