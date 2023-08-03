using Finbuckle.MultiTenant;

namespace ServerMultiTenant
{
    public class TenantHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<TenantHelper> logger;
        protected TenantInfo? CurrentTenent { get; private set; }
        public TenantHelper(IHttpContextAccessor accessor, ILogger<TenantHelper> logger)
        {
            httpContextAccessor = accessor;
            this.logger = logger;
        }
        public TenantInfo GetCurrentTenant()
        {
            try
            {
                var context = httpContextAccessor.HttpContext;
                var tenantContext = context?.GetMultiTenantContext<TenantInfo>();

                if (tenantContext?.TenantInfo == null)
                {
                    throw new TenantNotFoundException();
                }

                return tenantContext?.TenantInfo!;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new TenantNotFoundException();
            }
        }
    }
}
