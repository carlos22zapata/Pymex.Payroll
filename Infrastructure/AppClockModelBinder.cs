using Microsoft.AspNetCore.Mvc.ModelBinding;
using Pymex.Shared.Time;

namespace Pymex.Payroll.Infrastructure;

/// <summary>
/// Bind de DateTime/DateTime? desde query string o headers con semantica de zona horaria:
/// cadenas con Z/offset se toman como instante; cadenas naive se interpretan como hora pared
/// de la zona configurada (TZ) y se devuelven como Kind=Utc.
/// </summary>
public sealed class AppClockModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
        if (valueProviderResult == ValueProviderResult.None || string.IsNullOrWhiteSpace(valueProviderResult.FirstValue))
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var value = valueProviderResult.FirstValue!;
        if (AppClock.TryParseToUtc(value, out var utc))
        {
            bindingContext.Result = ModelBindingResult.Success(utc);
        }
        else
        {
            bindingContext.ModelState.AddModelError(
                bindingContext.ModelName,
                $"El valor '{value}' no es una fecha valida.");
            bindingContext.Result = ModelBindingResult.Failed();
        }
        return Task.CompletedTask;
    }
}

public sealed class AppClockModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        var type = context.Metadata.ModelType;
        return type == typeof(DateTime) || type == typeof(DateTime?)
            ? new AppClockModelBinder()
            : null;
    }
}


