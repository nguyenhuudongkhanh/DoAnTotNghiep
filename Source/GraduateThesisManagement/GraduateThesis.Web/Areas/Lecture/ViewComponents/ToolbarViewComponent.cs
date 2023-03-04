using Microsoft.AspNetCore.Mvc;

namespace GraduateThesis.Web.Areas.Lecture.ViewComponents;

[ViewComponent(Name = "FacultyStaffLayout_Toolbar")]
public class ToolbarViewComponent : ViewComponent
{
    public ToolbarViewComponent()
    {

    }

    public IViewComponentResult Invoke(bool showCreateButton = true, bool showExportButton = true, bool showImportButton = true, bool showPrintButton = true, bool showTrashButton = true)
    {
        ViewData["ShowCreateButton"] = showCreateButton;
        ViewData["ShowExportButton"] = showExportButton;
        ViewData["ShowImportButton"] = showImportButton;
        ViewData["ShowPrintButton"] = showPrintButton;
        ViewData["ShowTrashButton"] = showTrashButton;

        return View();
    }
}
