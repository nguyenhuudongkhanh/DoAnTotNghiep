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

namespace GraduateThesis.Web.Areas.Student.Controllers;

[Area("Student")]
[Route("student/account")]
public class StudentAccountController : WebControllerBase
{
    private IStudentRepository _studentRepository;
    private IAccountManager _accountManager;

    public StudentAccountController(IRepository repository, IAuthorizationManager authorizationManager)
    {
        _studentRepository = repository.StudentRepository;
        _accountManager = authorizationManager.AccountManager;
    }

    [Route("sign-in")]
    [HttpGet]
    [PageName(Name = "Trang đăng nhập dành cho sinh viên")]
    public IActionResult SignIn()
    {
        return View(new SignInModel());
    }

    [Route("sign-in")]
    [HttpPost]
    [PageName(Name = "Trang đăng nhập dành cho sinh viên")]
    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(AccountStatus.InvalidData);
            return View(signInModel);
        }

        SignInResultModel signInResultModel = await _studentRepository.SignInAsync(signInModel);

        if (signInResultModel.Status == AccountStatus.Success)
        {
            StudentOutput student = await _studentRepository.GetAsync(signInModel.Code);

            _accountManager.SetHttpContext(HttpContext);
            _accountManager.SetSession(new AccountSession
            {
                UserId = student.Id,
                AccountModel = student,
                Roles = "Student",
                LastSignInTime = DateTime.Now
            });

            return RedirectToAction("Index", "StudentThesis");
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

        AccountVerificationModel accountVerification = await _studentRepository
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

        NewPasswordModel newPasswordModel = await _studentRepository
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

        ForgotPasswordModel forgotPasswordModel = await _studentRepository
            .CreateNewPasswordAsync(newPasswordModel);

        if (forgotPasswordModel.Status == AccountStatus.Success)
        {
            AddTempData(forgotPasswordModel);
            return RedirectToAction("SignIn");
        }

        AddViewData(forgotPasswordModel);
        return View(newPasswordModel);
    }

    [Route("sign-out")]
    [HttpGet]
    [IsStudent]
    public IActionResult SignOutAccount()
    {
        _accountManager.SetHttpContext(HttpContext);
        _accountManager.RemoveSession();
        return RedirectToAction("SignIn", "StudentAccount");
    }

    [Route("get-profile")]
    [HttpGet]
    [IsStudent]
    [AccountInfo(typeof(StudentOutput))]
    [PageName(Name = "Thông tin sinh viên")]
    public async Task<IActionResult> GetProfile()
    {
        _accountManager.SetHttpContext(HttpContext);
        AccountSession accountSession = _accountManager.GetSession();
        StudentOutput student = await _studentRepository.GetAsync(accountSession.UserId);

        return View(student);
    }

    [Route("update-profile")]
    [HttpPost]
    [IsStudent]
    [AccountInfo(typeof(StudentOutput))]
    public async Task<IActionResult> UpdateProfile(IFormFile formFile, StudentInput studentInput)
    {
        if (!ModelState.IsValid)
        {
            AddTempData(DataResponseStatus.InvalidData);
            return RedirectToAction("GetProfile");
        }

        DataResponse dataResponse = null;
        if (formFile == null)
            dataResponse = await _studentRepository.UpdateProfileAsync(studentInput, null);
        else
            dataResponse = await _studentRepository
                .UpdateProfileAsync(studentInput, formFile.ToFileUploadModel());

        AddTempData(dataResponse);

        return RedirectToAction("GetProfile");
    }

    [Route("set-default-avatar")]
    [HttpPost]
    [IsStudent]
    [AccountInfo(typeof(StudentOutput))]
    public async Task<IActionResult> SetDefaultAvatar()
    {
        _accountManager.SetHttpContext(HttpContext);
        AccountSession accountSession = _accountManager.GetSession();
        DataResponse dataResponse = await _studentRepository.SetDefaultAvatarAsync(accountSession.UserId);

        AddTempData(dataResponse);

        return RedirectToAction("GetProfile");
    }
}
