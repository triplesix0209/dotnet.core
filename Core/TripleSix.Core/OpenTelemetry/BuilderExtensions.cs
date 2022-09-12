﻿using Microsoft.AspNetCore.Http;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.Http;
using OpenTelemetry.Instrumentation.Quartz;
using OpenTelemetry.Trace;
using Quartz.Impl;
using TripleSix.Core.Helpers;
using TripleSix.Core.OpenTelemetry.Shared;

namespace TripleSix.Core.OpenTelemetry
{
    /// <summary>
    /// Các Hàm hỗ trợ cấu hình OpenTelemetry.
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// Thêm <c>TripleSix.Core</c> vào danh sách Activity Source.
        /// </summary>
        /// <param name="builder"><see cref="TracerProviderBuilder"/> sẽ được cấu hình.</param>
        /// <returns><see cref="TracerProviderBuilder"/> sau khi được cấu hình.</returns>
        public static TracerProviderBuilder AddSourceTripleSixCore(this TracerProviderBuilder builder)
        {
            return builder.AddSource(ActivityHelper.SourceName);
        }

        /// <summary>
        /// Kích hoạt Instrumentation cho ASP.Core (mở rộng).
        /// </summary>
        /// <param name="builder"><see cref="TracerProviderBuilder"/> sẽ được cấu hình.</param>
        /// <param name="configureOptions">Hàm cấu hình với <see cref="AspNetCoreInstrumentationOptions"/>.</param>
        /// <returns><see cref="TracerProviderBuilder"/> sau khi được cấu hình.</returns>
        public static TracerProviderBuilder AddAspNetCoreInstrumentationEx(
            this TracerProviderBuilder builder,
            Action<AspNetCoreInstrumentationOptions>? configureOptions = default)
        {
            return builder.AddAspNetCoreInstrumentation(options =>
            {
                options.Enrich = async (activity, eventName, rawObject) =>
                {
                    if (eventName.Equals("OnStartActivity"))
                    {
                        if (rawObject is not HttpRequest httpRequest) return;

                        activity.SetTag("http.request_protocol", httpRequest.Protocol);
                        activity.SetTag("http.request_curl", await httpRequest.ToCurl());
                    }
                    else if (eventName.Equals("OnStopActivity"))
                    {
                        if (rawObject is not HttpResponse httpResponse) return;

                        activity.SetTag(SemanticConventions.AttributeHttpResponseContentLength, httpResponse.ContentLength);

                        var httpMethod = activity.GetTagItem(SemanticConventions.AttributeHttpMethod) as string;
                        if (!httpMethod.IsNullOrWhiteSpace())
                        {
                            if (!activity.DisplayName.StartsWith("/")) activity.DisplayName = "/" + activity.DisplayName;
                            activity.DisplayName = $"[{httpMethod}] " + activity.DisplayName;
                        }
                    }
                };

                configureOptions?.Invoke(options);
            });
        }

        /// <summary>
        /// Kích hoạt Instrumentation cho Microsoft.EntityFrameworkCore (mở rộng).
        /// </summary>
        /// <param name="builder"><see cref="TracerProviderBuilder"/> sẽ được cấu hình.</param>
        /// <param name="configureOptions">Hàm cấu hình với <see cref="EntityFrameworkCoreInstrumentationOptions"/>.</param>
        /// <returns><see cref="TracerProviderBuilder"/> sau khi được cấu hình.</returns>
        public static TracerProviderBuilder AddEntityFrameworkInstrumentationEx(
            this TracerProviderBuilder builder,
            Action<EntityFrameworkCoreInstrumentationOptions>? configureOptions = default)
        {
            var options = new EntityFrameworkCoreInstrumentationOptions();
            configureOptions?.Invoke(options);

            return builder.AddInstrumentation(() => new EntityFrameworkCoreInstrumentation(options));
        }

        /// <summary>
        /// Kích hoạt Instrumentation cho <see cref="HttpClient"/> (mở rộng).
        /// </summary>
        /// <param name="builder"><see cref="TracerProviderBuilder"/> sẽ được cấu hình.</param>
        /// <param name="configureOptions">Hàm cấu hình với <see cref="HttpClientInstrumentationOptions"/>.</param>
        /// <returns><see cref="TracerProviderBuilder"/> sau khi được cấu hình.</returns>
        public static TracerProviderBuilder AddHttpClientInstrumentationEx(
            this TracerProviderBuilder builder,
            Action<HttpClientInstrumentationOptions>? configureOptions = default)
        {
            return builder.AddHttpClientInstrumentation(options =>
            {
                options.SetHttpFlavor = true;
                options.RecordException = true;
                options.Enrich = async (activity, eventName, rawObject) =>
                {
                    if (eventName.Equals("OnStartActivity"))
                    {
                        if (rawObject is not HttpRequestMessage httpRequestMessage) return;

                        activity.SetTag("http.request_curl", await httpRequestMessage.ToCurl());
                    }
                    else if (eventName.Equals("OnStopActivity"))
                    {
                        if (rawObject is not HttpResponseMessage httpResponseMessage) return;

                        var host = httpResponseMessage.RequestMessage?.RequestUri?.Host;
                        if (!host.IsNullOrWhiteSpace() && host.StartsWith("www."))
                            host = host[4..];

                        activity.DisplayName = host.IsNullOrWhiteSpace() ? "<HTTP REQUEST>" : $"<HTTP> {host}";
                        activity.SetTag("http.response", await httpResponseMessage.Content.ReadAsStringAsync());
                    }
                };

                configureOptions?.Invoke(options);
            });
        }

        /// <summary>
        /// Kích hoạt Instrumentation cho <see cref="Quartz"/> (mở rộng).
        /// </summary>
        /// <param name="builder"><see cref="TracerProviderBuilder"/> sẽ được cấu hình.</param>
        /// <param name="configureOptions">Hàm cấu hình với <see cref="QuartzInstrumentationOptions"/>.</param>
        /// <returns><see cref="TracerProviderBuilder"/> sau khi được cấu hình.</returns>
        public static TracerProviderBuilder AddQuartzInstrumentationEx(
            this TracerProviderBuilder builder,
            Action<QuartzInstrumentationOptions>? configureOptions = default)
        {
            return builder.AddQuartzInstrumentation(options =>
            {
                options.Enrich = (activity, eventName, rawObject) =>
                {
                    if (!eventName.Equals("OnStopActivity")) return;
                    if (rawObject is not JobDetailImpl job) return;

                    activity.DisplayName = $"[Quartz] {job.Name}";
                };

                configureOptions?.Invoke(options);
            });
        }
    }
}
