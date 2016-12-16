using System;
using System.Security.Claims;
using BrockAllen.MembershipReboot;

namespace BL.Services.User
{
    public class AuthenticationWrapper : AuthenticationService<DAL.Entities.UserAccount>
    {
        #region tokenActions
        private Action<ClaimsPrincipal, TimeSpan?, bool?> issueTokenAction;

        private Action revokeTokenAction;
        #endregion

        public AuthenticationWrapper(UserAccountService<DAL.Entities.UserAccount> userService)
            : base(userService) { }

        public AuthenticationWrapper(UserAccountService<DAL.Entities.UserAccount> userService, ClaimsAuthenticationManager claimsAuthenticationManager) : base(userService, claimsAuthenticationManager) { }

        protected override ClaimsPrincipal GetCurentPrincipal()
        {
            return ClaimsPrincipal.Current;
        }

        #region TokensManagement
        public void InitializeIssueTokenAction(Action<ClaimsPrincipal, TimeSpan?, bool?> action)
        {
            if (issueTokenAction == null)
            {
                issueTokenAction = action;
            }
        }

        public void InitializeRevokeTokenAction(Action action)
        {
            if (revokeTokenAction == null)
            {
                revokeTokenAction = action;
            }
        }

        protected override void IssueToken(ClaimsPrincipal principal, TimeSpan? tokenLifetime = null, bool? persistentCookie = null)
        {
            if (issueTokenAction == null)
            {
                throw new InvalidOperationException("Issue token action has not been initialized yet!");
            }
            issueTokenAction.Invoke(principal, tokenLifetime, persistentCookie);
        }

        protected override void RevokeToken()
        {
            if (revokeTokenAction == null)
            {
                throw new InvalidOperationException("Issue token action has not been initialized yet!");
            }
            revokeTokenAction.Invoke();
        }
        #endregion

        #region SignInManagement
        public void PerformSignIn(Guid userId, bool rememberMe)
        {
            SignIn(userId, rememberMe);
        }

        public void PerformSignOut()
        {
            SignOut();
        }
        #endregion

    }
}