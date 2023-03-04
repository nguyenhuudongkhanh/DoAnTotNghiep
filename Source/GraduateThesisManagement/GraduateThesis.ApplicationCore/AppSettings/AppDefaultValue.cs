
#nullable disable

namespace GraduateThesis.ApplicationCore.AppSettings;

public class AppDefaultValue
{
    public static string ConnectionString = null;

    public static string AccountAssetsPath = null;
    public static string ThesisAssetsPath = null;

    public static string DbName = null;
    public static string DbBackupFilePath = null;

    public static bool ShowError = false;

    public static string SuccessMsg = null;
    public static string AlreadyExistsMsg = null;
    public static string NotFoundMsg = null;
    public static string HasConstraintMsg = null;
    public static string FailedMsg = null;
    public static string InvalidDataMsg = null;

    public static string AccAuthSuccessMsg = null;
    public static string AccAuthNotActivatedMsg = null;
    public static string AccAlreadyExistsMsg = null;
    public static string AccAuthLockedMsg = null;
    public static string AccAuthWrongPwdMsg = null;
    public static string AccAuthNotFoundMsg = null;
    public static string AccAuthInvalidDataMsg = null;
    public static string AccAuthFailedMsg = null;
}
