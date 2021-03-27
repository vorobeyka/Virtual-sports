using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtualSports.Lib.Models;

namespace VirtualSports.Web.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PlatformAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var inputValues = value as IEnumerable<string>;
            var isValid = inputValues
                .Select(value => value.ToLower())
                .All(value => AppTools.Platforms.Any(pl => value == pl));
            return isValid;
        }
    }
}
