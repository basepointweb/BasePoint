﻿using BasePoint.Core.Domain.Entities.Interfaces;
using Dapper;
using Newtonsoft.Json;
using System.Data;

namespace BasePoint.Core.Cqrs.Dapper.Extensions
{
    public static class DynamicParametersExtension
    {
        public static void AddAsJson<T>(this DynamicParameters parameters, string parameterName, T parameterValue, JsonSerializerSettings settings = null) where T : class
        {
            if (parameterValue is null)
                parameters.Add(parameterName);
            else
            {
                var jsonString = JsonConvert.SerializeObject(
                    parameterValue,
                    settings);

                parameters.Add(parameterName, jsonString, size: jsonString.Length);
            }
        }

        public static void AddAsJson<T>(this DynamicParameters parameters, string parameterName, T? parameterValue, JsonSerializerSettings settings = null) where T : struct
        {
            if (!parameterValue.HasValue)
                parameters.Add(parameterName);
            else
            {
                var jsonString = JsonConvert.SerializeObject(
                    parameterValue,
                    settings);

                parameters.Add(parameterName, jsonString, size: jsonString.Length);
            }
        }

        public static void AddNullable<T>(this DynamicParameters parameters, string parameterName, T? parameterValue, int? size = null) where T : struct
        {
            if (!parameterValue.HasValue)
                parameters.Add(parameterName, size: size);
            else
                parameters.Add(parameterName, parameterValue.Value, size: size);
        }

        public static void AddNullable(this DynamicParameters parameters, string parameterName, Guid? parameterValue, int? size = null)
        {
            if (!parameterValue.HasValue)
                parameters.Add(parameterName, size: size);
            else
                parameters.Add(parameterName, parameterValue.Value, DbType.Guid, size: size);
        }

        public static void AddNullable(this DynamicParameters parameters, string parameterName, string parameterValue, int? size = null)
        {
            if (parameterValue is null)
                parameters.Add(parameterName, DbType.AnsiString, size: size);
            else
                parameters.Add(parameterName, parameterValue, DbType.AnsiString, size: size);
        }

        public static void AddNullable(this DynamicParameters parameters, string parameterName, DateTime? parameterValue, int? size = null)
        {
            if (!parameterValue.HasValue)
                parameters.Add(parameterName, DbType.DateTime2, size: size);
            else
                parameters.Add(parameterName, parameterValue, DbType.DateTime2, size: size);
        }

        public static void AddNullable(this DynamicParameters parameters, string parameterName, DateTimeOffset? parameterValue, int? size = null)
        {
            if (!parameterValue.HasValue)
                parameters.Add(parameterName, DbType.DateTimeOffset, size: size);
            else
                parameters.Add(parameterName, parameterValue, DbType.DateTimeOffset, size: size);
        }

        public static void AddNullable(this DynamicParameters parameters, string parameterName, object parameterValue, int? size = null)
        {
            if (parameterValue is IBaseEntity)
                AddNullable(parameters, parameterName, (IBaseEntity)parameterValue, size);
            else
            {
                if (parameterValue is null)
                    parameters.Add(parameterName, size: size);
                else
                {
                    var parameterType = parameterValue.GetType();

                    if (parameterType.Name.ToUpper() == "STRING")
                        parameters.Add(parameterName, parameterValue, DbType.AnsiString, size: size);
                    else
                        parameters.Add(parameterName, parameterValue, size: size);
                }
            }
        }

        public static void AddNullable(this DynamicParameters parameters, string parameterName, IBaseEntity parameterValue, int? size = null)
        {
            if (parameterValue is null)
                parameters.Add(parameterName, size: size);
            else
                parameters.Add(parameterName, parameterValue.Id, DbType.Guid, size: size);
        }
    }
}