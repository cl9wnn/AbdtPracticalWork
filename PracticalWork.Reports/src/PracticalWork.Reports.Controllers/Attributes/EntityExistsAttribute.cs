using Microsoft.AspNetCore.Mvc;
using PracticalWork.Reports.Controllers.Filters;
using PracticalWork.Reports.SharedKernel.Abstractions;

namespace PracticalWork.Reports.Controllers.Attributes;

public class EntityExistsAttribute<TService, TDto> : TypeFilterAttribute
    where TService : class, IEntityService<TDto>
{
    public EntityExistsAttribute(string paramName = "id") : base(typeof(EntityExistsFilter<TService, TDto>))
    {
        Arguments = new object[] { paramName };
    }
}