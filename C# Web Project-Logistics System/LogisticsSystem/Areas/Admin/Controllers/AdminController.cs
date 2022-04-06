using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static LogisticsSystem.Areas.Admin.AdminConstants;

namespace LogisticsSystem.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public class AdminController:Controller
    {
    }
}
