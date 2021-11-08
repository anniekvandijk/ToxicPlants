﻿using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Function.MiddleWare
{
    public static class FunctionContextExtensions
    {
        public static HttpRequestData GetHttpRequestData(this FunctionContext functionContext)
        {
            try
            {
                KeyValuePair<Type, object> keyValuePair = functionContext.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
                object functionBindingsFeature = keyValuePair.Value;
                Type type = functionBindingsFeature.GetType();
                var inputData = type.GetProperties().Single(p => p.Name == "InputData").GetValue(functionBindingsFeature) as IReadOnlyDictionary<string, object>;
                return inputData?.Values.SingleOrDefault(o => o is HttpRequestData) as HttpRequestData;
            }
            catch
            {
                return null;
            }
        }
    }
}
