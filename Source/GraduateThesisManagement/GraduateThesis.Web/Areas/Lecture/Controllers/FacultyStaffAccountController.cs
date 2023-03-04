using GraduateThesis.ApplicationCore.AppController;
using GraduateThesis.ApplicationCore.Authorization;
using GraduateThesis.ApplicationCore.Enums;
using GraduateThesis.ApplicationCore.Models;
using GraduateThesis.ApplicationCore.WebAttributes;
using GraduateThesis.Common.WebAttributes;
using GraduateThesis.Repository.BLL.Interfaces;
using GraduateThesis.Repository.DTO;
using GraduateThesis.WebExtensions;
using Microsoft.AspNetCore.Mvc;

#nullable disable

namespace GraduateThesis.Web.Areas.Lecture.Controllers;

[Area("Lecture")]
[Route("lecture/account")]
public class FacultyStaffAccountController : WebControllerBase
{
    private IFacultyStaffRepository _facultyStaffRepository;
    private IAccountManager _accountManager;

    public FacultyStaffAccountController(IRepository repository, IAuthorizationManager authorizationManager)
    {
        _facultyStaffRepository = repository.FacultyStaffRepository;
        _accountManager = authorizationManager.AccountManager;
    }

    [Route("sign-in")]
    [HttpGet]
    [PageName(Name = "Trang đăng nhập dành cho giảng viên")]
    public IActionResult SignIn()
    {
        return View(new SignInModel());
    }

    [Route("sign-in")]
    [HttpPost]
    [PageName(Name = "Trang đăng nhập dành cho giảng viên")]
    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        if (!ModelState.IsValid)
        {
            AddViewData(AccountStatus.InvalidData);
            return View(signInModel);
        }

        SignInResultModel signInResultModel = await _facultyStaffRepository.SignInAsync(signInModel);

        if (signInResultModel.Status == AccountStatus.Success)
        {
            FacultyStaffOutput facultyStaff = await _facultyStaffRepository.GetAsync(signInModel.Code);
            _accountManager.SetHttpContext(HttpContext);

            _accountManager.SetSession(new AccountSession
            {
                UserId = facultyStaff.Id,
                LastSignInTime = DateTime.Now,
                AccountModel = facultyStaff
            });

            return RedirectToAction("Index", "FacultyStaffDashboard");
        }

        AddViewData(signInResultModel);
        return View(signInModel);
    }

    [Route("forgot-password")]
    [HttpGet]
    [PageName(Name = "Lấy lại mật khẩu")]
    public IActionResult ForgotPassword()
    {
        return View(new ForgotPasswordModel());
    }

    [Route("forgot-password")]
    [HttpPost]
    [PageName(Name = "Lấy lại mật khẩu")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        if (!ModelState.IsValid)
        {
            AddViewData(AccountStatus.InvalidData);
            return View(forgotPasswordModel);
        }

        AccountVerificationModel accountVerification = await _facultyStaffRepository
            .ForgotPasswordAsync(forgotPasswordModel);

        if (accountVerification.Status == AccountStatus.Success)
        {
            AddViewData(accountVerification);
            return View("Verify", accountVerification);
        }
            
        AddViewData(accountVerification);
        return View(forgotPasswordModel);
    }

    [Route("verify")]
    [HttpPost]
    [PageName(Name = "Lấy lại mật khẩu")]
    public async Task<IActionResult> Verify(AccountVerificationModel verificationModel)
    {
        if (!ModelState.IsValid)
        {
            AddViewData(AccountStatus.InvalidData);
            return View(verificationModel);
        }

        NewPasswordModel newPasswordModel = await _facultyStaffRepository
            .VerifyAccountAsync(verificationModel);

        if (newPasswordModel.Status == AccountStatus.Success)
        {
            AddViewData(newPasswordModel);
            return View("CreatePassword", newPasswordModel);
        }

        AddViewData(newPasswordModel);
        return View(verificationModel);
    }

    [Route("create-password")]
    [HttpPost]
    [PageName(Name = "Lấy lại mật khẩu")]
    public async Task<IActionResult> CreatePassword(NewPasswordModel newPasswordModel)
    {
        if (!ModelState.IsValid)
        {
            AddViewData(AccountStatus.InvalidData);
            return View(newPasswordModel);
        }

        ForgotPasswordModel forgotPasswordModel = await _facultyStaffRepository
            .CreateNewPasswordAsync(newPasswordModel);

        if(forgotPasswordModel.Status == AccountStatus.Success)
        {
            AddTempData(forgotPasswordModel);
            return RedirectToAction("SignIn");
        }

        AddViewData(forgotPasswordModel);
        return View(newPasswordModel);
    }

    [Route("sign-out")]
    [HttpGet]
    public IActionResult SignOutAccount()
    {
        _accountManager.SetHttpContext(HttpContext);
        _accountManager.RemoveSession();
        return RedirectToAction("Index", "Home");
    }

    [Route("get-profile")]
    [HttpGet]
    [WebAuthorize]
    [AccountInfo(typeof(FacultyStaffOutput))]
    [PageName(Name = "Thông tin giảng viên")]
    public async Task<IActionResult> GetProfile()
    {
        _accountManager.SetHttpContext(HttpContext);
        AccountSession accountSession = _accountManager.GetSession();
        FacultyStaffOutput facultyStaffOutput = await _facultyStaffRepository.GetAsync(accountSession.UserId);

        return View(facultyStaffOutput);
    }

    [Route("update-profile")]
    [HttpPost]
    [WebAuthorize]
    [AccountInfo(typeof(FacultyStaffOutput))]
    public async Task<IActionResult> UpdateProfile(IFormFile formFile, FacultyStaffInput facultyStaffInput)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("GetProfile");
        }

        DataResponse dataResponse = null;
        if (formFile == null)
            dataResponse = await _facultyStaffRepository.UpdateProfileAsync(facultyStaffInput, null);
        else
            dataResponse = await _facultyStaffRepository
                .UpdateProfileAsync(facultyStaffInput, formFile.ToFileUploadModel());

        AddTempData(dataResponse);

        return RedirectToAction("GetProfile");
    }

    [Route("set-default-avatar")]
    [HttpPost]
    [WebAuthorize]
    [AccountInfo(typeof(FacultyStaffOutput))]
    public async Task<IActionResult> SetDefaultAvatar()
    {
        _accountManager.SetHttpContext(HttpContext);
        AccountSession accountSession = _accountManager.GetSession();
        DataResponse dataResponse = await _facultyStaffRepository.SetDefaultAvatarAsync(accountSession.UserId);

        AddTempData(dataResponse);

        return RedirectToAction("GetProfile");
    }
}
