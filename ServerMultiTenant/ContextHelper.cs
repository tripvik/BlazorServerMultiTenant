using Finbuckle.MultiTenant;
using ServerMultiTenant.Data;

namespace ServerMultiTenant
{
    public class ContextHelper
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ApplicationDbContext _dbContext;
        public ContextHelper(IHttpContextAccessor accessor, ApplicationDbContext dbContext)
        {
            _accessor = accessor;
            _dbContext = dbContext;
        }
        public TenantInfo GetCurrentTenant()
        {
            var context = _accessor.HttpContext;
            var ti = context.GetMultiTenantContext<TenantInfo>().TenantInfo;
            return ti;
        }
        public List<ToDoItem> GetToDoItems()
        {
            var context = _accessor.HttpContext;
            List<ToDoItem> toDoItems = new List<ToDoItem>();
            var v1 = context.GetMultiTenantContext<TenantInfo>();
            if (v1.TenantInfo != null)
            {
                toDoItems = _dbContext.ToDoItems.ToList();
            }
            return toDoItems;
        }
        public string GetTenantName()
        {
            try
            {
                var context = _accessor.HttpContext;
                var v1 = context.GetMultiTenantContext<TenantInfo>();
                if (v1.TenantInfo != null)
                {
                    return v1.TenantInfo.Name;
                }
            }
            catch (Exception x)
            { return @"Exception:Unknown"; }
            return @"Unknown";
        }
    }
}
