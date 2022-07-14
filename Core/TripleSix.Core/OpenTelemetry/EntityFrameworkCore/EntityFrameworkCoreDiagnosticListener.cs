using System.Data;
using System.Diagnostics;
using OpenTelemetry.Trace;
using TripleSix.Core.Helpers;
using TripleSix.Core.OpenTelemetry.Shared;

namespace TripleSix.Core.OpenTelemetry
{
    internal sealed class EntityFrameworkCoreDiagnosticListener : ListenerHandler
    {
        #region [fields]

        internal const string DiagnosticSourceName = "Microsoft.EntityFrameworkCore";
        internal static readonly string ActivityName = ActivityHelper.SourceName + ".Instrumentation.EntityFrameworkCore";

        private const string EntityFrameworkCoreCommandCreated = "Microsoft.EntityFrameworkCore.Database.Command.CommandCreated";
        private const string EntityFrameworkCoreCommandExecuting = "Microsoft.EntityFrameworkCore.Database.Command.CommandExecuting";
        private const string EntityFrameworkCoreCommandExecuted = "Microsoft.EntityFrameworkCore.Database.Command.CommandExecuted";
        private const string EntityFrameworkCoreCommandError = "Microsoft.EntityFrameworkCore.Database.Command.CommandError";

        private const string AttributeEfProvider = "ef.provider";

        private readonly PropertyFetcher<object> commandFetcher = new PropertyFetcher<object>("Command");
        private readonly PropertyFetcher<object> connectionFetcher = new PropertyFetcher<object>("Connection");
        private readonly PropertyFetcher<object> dbContextFetcher = new PropertyFetcher<object>("Context");
        private readonly PropertyFetcher<object> dbContextDatabaseFetcher = new PropertyFetcher<object>("Database");
        private readonly PropertyFetcher<string> providerNameFetcher = new PropertyFetcher<string>("ProviderName");
        private readonly PropertyFetcher<object> dataSourceFetcher = new PropertyFetcher<object>("DataSource");
        private readonly PropertyFetcher<object> databaseFetcher = new PropertyFetcher<object>("Database");
        private readonly PropertyFetcher<CommandType> commandTypeFetcher = new PropertyFetcher<CommandType>("CommandType");
        private readonly PropertyFetcher<string> commandTextFetcher = new PropertyFetcher<string>("CommandText");
        private readonly PropertyFetcher<Exception> exceptionFetcher = new PropertyFetcher<Exception>("Exception");

        private readonly EntityFrameworkCoreInstrumentationOptions options;

        #endregion

        public EntityFrameworkCoreDiagnosticListener(string sourceName, EntityFrameworkCoreInstrumentationOptions? options)
            : base(sourceName)
        {
            this.options = options ?? new EntityFrameworkCoreInstrumentationOptions();
        }

        public override bool SupportsNullActivity => true;

        public override void OnCustom(string eventName, Activity? activity, object? payload)
        {
            switch (eventName)
            {
                case EntityFrameworkCoreCommandCreated:
                    {
                        activity = ActivityHelper.ActivitySource.StartActivity(ActivityName, ActivityKind.Client);
                        if (activity == null) return;

                        var command = commandFetcher.Fetch(payload);
                        if (command == null)
                        {
                            EntityFrameworkCoreInstrumentationEventSource.Log.NullPayload(nameof(EntityFrameworkCoreDiagnosticListener), eventName);
                            activity.Stop();
                            return;
                        }

                        var connection = connectionFetcher.Fetch(command);
                        var database = databaseFetcher.Fetch(connection) as string;
                        activity.DisplayName = $"[db] " + (database ?? string.Empty);
                        if (!activity.IsAllDataRequested) return;

                        var dataSource = dataSourceFetcher.Fetch(connection) as string;
                        var dbContext = dbContextFetcher.Fetch(payload);
                        var dbContextDatabase = dbContextDatabaseFetcher.Fetch(dbContext);
                        var providerName = providerNameFetcher.Fetch(dbContextDatabase);

                        var dbSystem = providerName switch
                        {
                            "Microsoft.EntityFrameworkCore.SqlServer" => "mssql",
                            "Microsoft.EntityFrameworkCore.Cosmos" => "cosmosdb",
                            "Microsoft.EntityFrameworkCore.Sqlite" => "sqlite",
                            "MySql.Data.EntityFrameworkCore" or "Pomelo.EntityFrameworkCore.MySql" => "mysql",
                            "Npgsql.EntityFrameworkCore.PostgreSQL" => "postgresql",
                            "Oracle.EntityFrameworkCore" => "oracle",
                            _ => "db",
                        };

                        activity.DisplayName = $"[{dbSystem}] " + (database ?? string.Empty);
                        activity.AddTag(SemanticConventions.AttributeDbName, database);
                        activity.AddTag(SemanticConventions.AttributeDbSystem, dbSystem);
                        activity.AddTag(AttributeEfProvider, providerName);
                        if (!dataSource.IsNullOrWhiteSpace())
                            activity.AddTag(SemanticConventions.AttributePeerService, dataSource);
                    }

                    break;

                case EntityFrameworkCoreCommandExecuting:
                    {
                        if (activity == null)
                        {
                            EntityFrameworkCoreInstrumentationEventSource.Log.NullActivity(eventName);
                            return;
                        }

                        if (activity.Source != ActivityHelper.ActivitySource)
                            return;

                        if (!activity.IsAllDataRequested)
                            return;

                        var command = commandFetcher.Fetch(payload);
                        if (commandTypeFetcher.Fetch(command) is CommandType commandType)
                        {
                            var commandText = commandTextFetcher.Fetch(command);
                            switch (commandType)
                            {
                                case CommandType.StoredProcedure:
                                    activity.AddTag(SpanAttributeConstants.DatabaseStatementTypeKey, nameof(CommandType.StoredProcedure));
                                    if (options.SetDbStatementForStoredProcedure)
                                        activity.AddTag(SemanticConventions.AttributeDbStatement, commandText);
                                    break;

                                case CommandType.Text:
                                    activity.AddTag(SpanAttributeConstants.DatabaseStatementTypeKey, nameof(CommandType.Text));
                                    if (options.SetDbStatementForText)
                                        activity.AddTag(SemanticConventions.AttributeDbStatement, commandText);
                                    break;

                                case CommandType.TableDirect:
                                    activity.AddTag(SpanAttributeConstants.DatabaseStatementTypeKey, nameof(CommandType.TableDirect));
                                    break;
                            }
                        }
                    }

                    break;

                case EntityFrameworkCoreCommandExecuted:
                    {
                        if (activity == null)
                        {
                            EntityFrameworkCoreInstrumentationEventSource.Log.NullActivity(eventName);
                            return;
                        }

                        if (activity.Source != ActivityHelper.ActivitySource)
                            return;

                        activity.Stop();
                    }

                    break;

                case EntityFrameworkCoreCommandError:
                    {
                        if (activity == null)
                        {
                            EntityFrameworkCoreInstrumentationEventSource.Log.NullActivity(eventName);
                            return;
                        }

                        if (activity.Source != ActivityHelper.ActivitySource)
                            return;

                        try
                        {
                            if (activity.IsAllDataRequested)
                            {
                                if (exceptionFetcher.Fetch(payload) is Exception exception)
                                    activity.SetStatus(Status.Error.WithDescription(exception.Message));
                                else
                                    EntityFrameworkCoreInstrumentationEventSource.Log.NullPayload(nameof(EntityFrameworkCoreDiagnosticListener), eventName);
                            }
                        }
                        finally
                        {
                            activity.Stop();
                        }
                    }

                    break;
            }
        }
    }
}
