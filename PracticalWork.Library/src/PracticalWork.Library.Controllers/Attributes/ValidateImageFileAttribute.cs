using Microsoft.AspNetCore.Mvc;
using PracticalWork.Library.Controllers.Filters;

namespace PracticalWork.Library.Controllers.Attributes;

public class ValidateImageFileAttribute: TypeFilterAttribute
{
    public ValidateImageFileAttribute(long maxFileSizeBytes)
        : base(typeof(ValidateImageFileFilter))
    {
        Arguments = new object[] { maxFileSizeBytes };
    }}