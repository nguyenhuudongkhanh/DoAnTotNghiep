using GraduateThesis.ApplicationCore.Models;

namespace GraduateThesis.ApplicationCore.Repository;

/// <summary>
/// Author: phanxuanchanh.com (phanchanhvn)
/// </summary>

public interface IAsyncAccountPattern
{
    Task<SignInResultModel> SignInAsync(SignInModel signInModel);
    Task<AccountVerificationModel> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
    Task<NewPasswordModel> VerifyAccountAsync(AccountVerificationModel accountVerificationModel);
    Task<ForgotPasswordModel> CreateNewPasswordAsync(NewPasswordModel newPasswordModel);
}
