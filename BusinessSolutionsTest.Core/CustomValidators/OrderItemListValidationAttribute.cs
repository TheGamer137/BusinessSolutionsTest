using System.ComponentModel.DataAnnotations;
using BusinessSolutionsTest.Core.Models;

namespace BusinessSolutionsTest.Core.CustomValidators;

[AttributeUsage(AttributeTargets.Property)]
public class OrderItemListValidationAttribute : ValidationAttribute
{
    private readonly string otherProperty;

    public OrderItemListValidationAttribute(string otherProperty)
    {
        this.otherProperty = otherProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var otherPropertyValue = validationContext.ObjectType.GetProperty(otherProperty)?.GetValue(validationContext.ObjectInstance, null);
        
        if (otherPropertyValue is List<OrderItem> list && list.Any())
        {
            var otherPropertyValues = list.Select(oi => oi.Name);

            if (value != null && otherPropertyValues.Contains(value))
            {
                return new ValidationResult($"{validationContext.DisplayName} не может быть равен названию элемента заказа.");
            }
        }

        return ValidationResult.Success;
    }
}
