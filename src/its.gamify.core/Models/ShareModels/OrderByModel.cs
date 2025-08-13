using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace its.gamify.core.Models.ShareModels
{
    public class OrderParam
    {
        public string? OrderColumn { get; set; }
        public string? OrderDir { get; set; } = "ASC";
    }

    public class FilterQuery
    {
        [FromQuery(Name = "page")]
        public int? Page { get; set; } = 0;
        [FromQuery(Name = "limit")]
        public int? Limit { get; set; } = 10;
        [FromQuery(Name = "q")]
        public string? Q { get; set; } = string.Empty;

        [FromQuery(Name = "order_by")]
        // [ModelBinder(BinderType = typeof(OrderParamListBinder))]
        public OrderParam[]? OrderBy { get; set; }
    }
    public class OrderParamListBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(OrderParam[]))
            {
                return new BinderTypeModelBinder(typeof(OrderParamListBinder));
            }
            return null;
        }
    }


    public class OrderParamListBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var result = new List<OrderParam>();
            var index = 0;

            while (true)
            {
                var prefix = $"order_by[{index}]";
                var columnKey = $"{prefix}[order_column]";
                var dirKey = $"{prefix}[order_dir]";

                var columnValue = bindingContext.ValueProvider.GetValue(columnKey).FirstValue;
                var dirValue = bindingContext.ValueProvider.GetValue(dirKey).FirstValue;

                if (string.IsNullOrEmpty(columnValue)) break;

                result.Add(new OrderParam
                {
                    OrderColumn = columnValue,
                    OrderDir = dirValue ?? "ASC"
                });

                index++;
            }

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }

}
