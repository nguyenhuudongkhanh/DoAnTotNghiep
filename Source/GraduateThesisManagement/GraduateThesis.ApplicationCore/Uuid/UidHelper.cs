using GraduateThesis.ApplicationCore.Uuid.ShortUid;

namespace GraduateThesis.ApplicationCore.Uuid;

public class UidHelper
{
    public static string GetShortUid()
    {
        return ShortUidHelper.Generate();
    }

    public static string GetMicrosoftUid()
    {
        return Guid.NewGuid().ToString();
    }
}
