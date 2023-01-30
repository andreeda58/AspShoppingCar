using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AspProject_Entities.ModelsValidation
{
    class BirthDateValidation : ValidationAttribute
    {
        public override bool IsValid(object value)
        => (value is DateTime) && (value as DateTime?).Value.Year > 1900 && (value as DateTime?) < DateTime.Now;
    }
}
