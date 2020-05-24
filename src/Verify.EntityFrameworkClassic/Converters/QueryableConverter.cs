﻿using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Verify;

class QueryableConverter :
    WriteOnlyJsonConverter
{
    public override void WriteJson(JsonWriter writer, object? data, JsonSerializer serializer)
    {
        if (data == null)
        {
            return;
        }

        var sql = QueryToSql(data);
        serializer.Serialize(writer, sql);
    }

    public static string QueryToSql(object data)
    {
        var entityType = data.GetType().GetGenericArguments().Single();
        var queryableSerializer = typeof(QueryableSerializer<>).MakeGenericType(entityType);
        return (string) queryableSerializer.InvokeMember(
            name: "ToSql",
            invokeAttr: BindingFlags.InvokeMethod,
            binder: null,
            target: null,
            args: new[] {data});
    }

    public override bool CanConvert(Type type)
    {
        return IsQueryable(type);
    }

    public static bool IsQueryable(object target)
    {
        return target is IQueryable;
    }
}