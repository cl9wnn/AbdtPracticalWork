using Microsoft.AspNetCore.Mvc;
using PracticalWork.Library.Controllers.Filters;

namespace PracticalWork.Library.Controllers.Attributes;

public class ValidateImageFileAttribute: TypeFilterAttribute
{
    public ValidateImageFileAttribute(): base(typeof(ValidateImageFileFilter)) { }
}