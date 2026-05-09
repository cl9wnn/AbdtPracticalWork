using Microsoft.AspNetCore.Mvc;
using PracticalWork.Library.Controllers.Filters;
using PracticalWork.Shared.Abstractions.Interfaces;

namespace PracticalWork.Library.Controllers.Attributes;

public class EntityExistsAttribute<TService, TDto> : TypeFilterAttribute
    where TService : class, IEntityService<TDto>
{
    public EntityExistsAttribute(string paramName = "id") : base(typeof(EntityExistsFilter<TService, TDto>))
    {
        Arguments = new object[] { paramName };
    }
}