// Licensed to acceliox GmbH under one or more agreements.
// See the LICENSE file in the project root for more information.Copyright (c) acceliox GmbH. All rights reserved.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp;

public static class VaultExtensions
{
    public static T? ToType<T>(this IDictionary<string, object> dict, string objName)
    {
        dict.TryGetValue(objName, out var obj);
        var objInJson = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject<T>(objInJson);
    }
}