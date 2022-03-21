using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ExplorJobAPI.DAL.Entities.Users;
using ExplorJobAPI.Domain.Models.AuthUsers;
using ExplorJobAPI.Domain.Queries.AuthUsers;
using ExplorJobAPI.Domain.Repositories.Users;
using ExplorJobAPI.Domain.Services.AuthUsers;
using ExplorJobAPI.Domain.Exceptions.AuthUsers;
using ExplorJobAPI.DAL.Mappers.Users;
using ExplorJobAPI.Domain.Commands.AuthUsers;
using ExplorJobAPI.Domain.Services.Emails;
using ExplorJobAPI.Domain.Commands.Emails;
using ExplorJobAPI.Domain.Repositories.Contracts;
using ExplorJobAPI.Domain.Commands.Contracts;
using ExplorJobAPI.Infrastructure.Repositories;
using ExplorJobAPI.Infrastructure.Http.Services;

namespace ExplorJobAPI.DAL.Services.AuthUsers
{
    public class AuthUsersService : IAuthUsersService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IUsersRepository _usersRepository;
        private readonly UserMapper _userMapper;
        private readonly ISendEmailService _sendEmailService;
        private readonly IContractUserAcceptancesRepository _contractUserAcceptancesRepository;
        private readonly UrlEncoder _urlEncoder;
        
        public AuthUsersService(
            UserManager<UserEntity> userManager,
            IUsersRepository usersRepository,
            UserMapper userMapper,
            ISendEmailService sendEmailService,
            IContractUserAcceptancesRepository contractUserAcceptancesRepository,
            UrlEncoder urlEncoder
        ) {
            _userManager = userManager;
            _usersRepository = usersRepository;
            _userMapper = userMapper;
            _sendEmailService = sendEmailService;
            _contractUserAcceptancesRepository = contractUserAcceptancesRepository;
            _urlEncoder = urlEncoder;
        }

        public async Task<AuthTokenUser> Login(
            UserLoginQuery query
        ) {
            var user = await _userManager.FindByEmailAsync(
                query.Email
            );

            if (user != null) {
                if (user.EmailConfirmed) {
                    if (await _userManager.CheckPasswordAsync(
                        user,
                        query.Password
                    )) {
                        return new AuthTokenUser(
                            await _usersRepository.FindOneByEmail(
                                query.Email
                            )
                        );
                    }
                    else {
                        throw new UserInvalidPasswordException(
                            query.Email
                        );
                    }
                }
                else {
                    throw new UserEmailNotConfirmedException(
                        query.Email
                    );
                }
            }
            else {
                throw new UserNotFoundException(
                    query.Email
                );
            }
        }

        public async Task<bool> Register(
            UserRegisterCommand command
        ) {
            UserEntity user = _userMapper.RegisterCommandToEntity(
                command
            );

            IdentityResult result = await _userManager.CreateAsync(
                user,
                command.Password
            );

            if (result.Succeeded) {
                command.ContractIds.Select(
                    async (contractId) => await AcceptContractUser(
                        contractId,
                        user.Guid.ToString()
                    )
                );

                bool emailWasSent = await SendEmailConfirmationToken(
                    user,
                    command.FallbackUrl
                );

                if (!emailWasSent) {
                    await _userManager.DeleteAsync(
                        user
                    );
                }

                return emailWasSent;
            }
            else {
                return false;
            }
        }

        public async Task<bool> GetNewEmailTokenConfirmation(
            UserGetNewEmailTokenConfirmationCommand command
        ) {
            UserEntity user = await _userManager.FindByEmailAsync(
                command.Email
            );

            if (user != null) {
                return await SendEmailConfirmationToken(
                    user,
                    command.FallbackUrl
                );
            }
            else {
                throw new UserNotFoundException(
                    command.Email
                );
            }
        }

        public async Task<bool> ConfirmEmail(
            UserConfirmEmailCommand command
        ) {
            UserEntity user = await _userManager.FindByEmailAsync(
                command.Email
            );

            if (user != null) {
                IdentityResult result = await _userManager.ConfirmEmailAsync(
                    user,
                    command.Token
                );

                return result.Succeeded;
            }
            else {
                throw new UserNotFoundException(
                    command.Email
                );
            }
        }

        public async Task<bool> ChangePassword(
            UserChangePasswordCommand command
        ) {
            var user = await _userManager.FindByEmailAsync(
                command.Email
            );

            if (user != null) {
                var changePassword = await _userManager.ChangePasswordAsync(
                    user,
                    command.CurrentPassword,
                    command.NewPassword
                );

                return changePassword.Succeeded;
            }
            else {
                throw new UserNotFoundException(
                    command.Email
                );
            }
        }

        public async Task<bool> ForgottenPassword(
            UserForgottenPasswordCommand command
        ) {
            UserEntity user = await _userManager.FindByEmailAsync(
                command.Email
            );

            if (user != null) {
                return await SendPasswordResetToken(
                    user,
                    command.FallbackUrl
                );
            }
            else {
                throw new UserNotFoundException(
                    command.Email
                );
            }
        }

        public async Task<bool> ResetPassword(
            UserResetPasswordCommand command
        ) {
            UserEntity user = await _userManager.FindByEmailAsync(
                _urlEncoder.Decode(command.Email)
            );


            if (user != null) {
                IdentityResult result = await _userManager.ResetPasswordAsync(
                    user,
                    _urlEncoder.Decode(command.Token),
                    command.NewPassword
                );

                return result.Succeeded;
            }
            else {
                throw new UserNotFoundException(
                    command.Email
                );
            }
        }

        private async Task<RepositoryCommandResponse> AcceptContractUser(
            string contractId,
            string userId
        ) {
            return await _contractUserAcceptancesRepository.Create(
                new ContractUserAcceptanceCreateCommand {
                    ContractId = contractId,
                    UserId = userId
                }
            );
        }

        private async Task<bool> SendEmailConfirmationToken(
            UserEntity user,
            string fallbackUrl
        ) {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(
                user
            );

            return await _sendEmailService.SendOne(
                new SendEmailCommand() {
                    Email = user.Email,
                    Subject = "ExplorJob - Confirmez votre email",
                    Message = $"<div>Bonjour,</div><br><div>Vous venez de vous inscrire sur ExplorJob, pour finaliser votre inscription, veuillez confirmer votre email.</div><br><div align=\"center\"><a href=\"{ BuildUrl(fallbackUrl, user.Email, token) }\" style=\"padding: 10px 15px 8.5px 15px; font-weight: bold; color: #f07434; background-color: #ffffff; border: 2px solid #f07434; outline: inherit; text-decoration: none; cursor: pointer;\">Confirmer mon email</a></div><br><div><strong>L'équipe ExplorJob</strong></div>",
                    IsHtml = true
                }
            );
        }

        private async Task<bool> SendPasswordResetToken(
            UserEntity user,
            string fallbackUrl
        ) {
            string token = await _userManager.GeneratePasswordResetTokenAsync(
                user
            );

            return await _sendEmailService.SendOne(
                new SendEmailCommand() {
                    Email = user.Email,
                    Subject = "ExplorJob - Confirmez la réinitialisation de mot de passe",
                    Message = $"<div>Bonjour,</div><br><div>Vous venez de demander la réinitialisation de votre mot de passe sur ExplorJob, si vous n'êtes pas à l'origine de cette requête, <a href=\"mailto:support@explorjob.com\">signalez-le nous</a>.</div><br><div align=\"center\"><a href=\"{ BuildUrl(fallbackUrl, user.Email, token) }\" style=\"padding: 10px 15px 8.5px 15px; font-weight: bold; color: #f07434; background-color: #ffffff; border: 2px solid #f07434; outline: inherit; text-decoration: none; cursor: pointer;\">Réinitialiser mon mot de passe</a></div><br><div><strong>L'équipe ExplorJob</strong></div>",
                    IsHtml = true
                }
            );
        }

        private string BuildUrl(
            string fallbackUrl,
            string email,
            string token
        ) {
            return $"{ fallbackUrl }/{ _urlEncoder.Encode(email) }/{ _urlEncoder.Encode(token) }";
        }
    }
}
