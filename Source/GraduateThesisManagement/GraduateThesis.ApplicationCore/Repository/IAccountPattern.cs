using GraduateThesis.ApplicationCore.Models;

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>

public interface IAccountPattern
{
    SignInResultModel SignIn(SignInModel signInModel);
    AccountVerificationModel ForgotPassword(ForgotPasswordModel forgotPasswordModel);
    NewPasswordModel VerifyAccount(AccountVerificationModel accountVerificationModel);
    ForgotPasswordModel CreateNewPassword(NewPasswordModel newPasswordModel);
}
