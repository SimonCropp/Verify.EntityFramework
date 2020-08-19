﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace VerifyTests
{
    public static class VerifyEntityFramework
    {
        public static void EnableRecording(this DbContextOptionsBuilder builder)
        {
            Guard.AgainstNull(builder, nameof(builder));
            builder.AddInterceptors(new LogCommandInterceptor());
        }

        public static void StartRecording<T>(this T data)
            where T : DbContext
        {
            Guard.AgainstNull(data, nameof(data));
            var interceptor = GetInterceptor(data);
            interceptor.Start();
        }

        static LogCommandInterceptor GetInterceptor<T>(T data)
            where T : DbContext
        {
            var provider = ((IInfrastructure<IServiceProvider>) data).Instance;
            var extension = provider.GetRequiredService<IDbContextOptions>()
                .Extensions
                .OfType<CoreOptionsExtension>()
                .Single();
            return extension.Interceptors
                .OfType<LogCommandInterceptor>()
                .Single();
        }

        public static IEnumerable<CommandEndEventData> FinishRecording<T>(this T data)
            where T : DbContext
        {
            Guard.AgainstNull(data, nameof(data));
            var interceptor = GetInterceptor(data);
            return interceptor.Stop();
        }

        public static void Enable()
        {
            VerifierSettings.RegisterFileConverter(
                QueryableToSql,
                (target, settings) => QueryableConverter.IsQueryable(target));
            VerifierSettings.ModifySerialization(settings =>
            {
                settings.AddExtraSettings(serializer =>
                {
                    var converters = serializer.Converters;
                    converters.Add(new ExecutedEventDataConverter());
                    converters.Add(new ErrorEventDataConverter());
                    converters.Add(new TrackerConverter());
                    converters.Add(new QueryableConverter());
                });
            });
        }

        static ConversionResult QueryableToSql(object arg, VerifySettings settings)
        {
            var sql = QueryableConverter.QueryToSql(arg);
            return new ConversionResult(
                null,
                new[]
                {
                    new ConversionStream("txt", StringToMemoryStream(sql)),
                });
        }

        static MemoryStream StringToMemoryStream(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text.Replace("\r\n", "\n"));
            return new MemoryStream(bytes);
        }
    }
}