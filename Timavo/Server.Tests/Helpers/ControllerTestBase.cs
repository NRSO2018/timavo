using System.Threading.Tasks;
using Timavo.Server.Data;

namespace Timavo.Server.Tests.Helpers
{
    public abstract class ControllerTestBase<T>
    {
        protected abstract Task<T> CreateController(ApplicationDbContext? applicationDbContext = null, string userId = "ApplicationUser1");

    }
}
