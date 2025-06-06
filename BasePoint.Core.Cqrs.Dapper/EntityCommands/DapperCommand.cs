﻿using BasePoint.Core.Cqrs.Dapper.Extensions;
using BasePoint.Core.Cqrs.Dapper.TableDefinitions;
using BasePoint.Core.Domain.Cqrs;
using BasePoint.Core.Domain.Entities.Interfaces;
using BasePoint.Core.Domain.Enumerators;
using BasePoint.Core.Exceptions;
using BasePoint.Core.Extensions;
using BasePoint.Core.Shared;
using Dapper;
using LinFu.DynamicProxy;
using System.Data;
using static Dapper.SqlMapper;

namespace BasePoint.Core.Cqrs.Dapper.EntityCommands
{
    public class DapperCommand<Entity> : IEntityCommand where Entity : IBaseEntity
    {
        protected class EntityTableMapping
        {
            public DapperTableDefinition TableDefinition { get; set; }
            public Dictionary<string, IBaseEntity> ParentEntities { get; set; }

            public EntityTableMapping()
            {
                ParentEntities = new Dictionary<string, IBaseEntity>();
            }

            public EntityTableMapping WithParentEntity(string entityFieldName, IBaseEntity baseEntity)
            {
                ParentEntities[entityFieldName] = baseEntity;

                return this;
            }
        };

        const char DAPPER_PARAMETER_INDICATOR = '@';

        protected readonly IDbConnection _connection;
        public IList<CommandDefinition> CommandDefinitions { get; protected set; }

        protected readonly Dictionary<string, EntityTableMapping> _entityTableTypeMappings;
        public IBaseEntity AffectedEntity { get; }
        public DapperCommand(IDbConnection connection, Entity affectedEntity)
        {
            _connection = connection;
            AffectedEntity = affectedEntity;
            CommandDefinitions = new List<CommandDefinition>();
            _entityTableTypeMappings = new Dictionary<string, EntityTableMapping>();
        }

        public virtual async Task<bool> ExecuteAsync()
        {
            bool sucess;
            try
            {
                CreateCommandDefinitions((Entity)AffectedEntity);

                foreach (var commandDefinition in CommandDefinitions)
                {
                    await _connection.ExecuteAsync(commandDefinition);
                }

                sucess = true;
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(Constants.ErrorMessages.DefaultErrorMessage + Constants.StringEnter + ex.Message);
            }

            return sucess;
        }

        protected EntityTableMapping AddTypeMapping(string typeName, DapperTableDefinition tableDefinition)
        {
            var entityTypeMapping = new EntityTableMapping() { TableDefinition = tableDefinition };

            _entityTableTypeMappings.Add(typeName, entityTypeMapping);

            return entityTypeMapping;
        }

        protected void AddCommandDefinition(CommandDefinition? commandDefinition)
        {
            if (commandDefinition.HasValue)
                CommandDefinitions.Add(commandDefinition.Value);
        }

        public CommandDefinition CreateAnInsertCommandFromParameters(
            Dictionary<string, object> entityParameters,
            DapperTableDefinition tableDefinition)
        {
            var insertScript = "Insert Into " + tableDefinition.TableName + "(" + Constants.StringEnter;

            var fieldsToInsert = string.Empty;
            var parameterNames = string.Empty;

            var dapperParameters = new DynamicParameters();

            foreach (var entityParameter in entityParameters)
            {
                var tableColumnDefinition = GetTableColumnDefinition(tableDefinition.ColumnDefinitions, entityParameter.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!fieldsToInsert.IsEmpty())
                    {
                        fieldsToInsert += Constants.StringComma + Constants.StringEnter;
                        parameterNames += Constants.StringComma + Constants.StringEnter;
                    }

                    fieldsToInsert += tableColumnDefinition.DbFieldName;
                    parameterNames += DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;

                    var objectValue = entityParameter.Value;

                    if (objectValue is IBaseEntity)
                        dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, (IBaseEntity)objectValue, size: tableColumnDefinition.Size);
                    else
                        dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, GetParameterValue(entityParameter.Key, entityParameter.Value), size: tableColumnDefinition.Size);
                }
            }

            insertScript += fieldsToInsert + ")" + Constants.StringEnter + "Values(" + Constants.StringEnter;

            insertScript += parameterNames + ");";

            var commandDefinition = new CommandDefinition(insertScript, dapperParameters);

            AddCommandDefinition(commandDefinition);

            return commandDefinition;
        }

        private static DapperTableColumnDefinition GetTableColumnDefinition(List<DapperTableColumnDefinition> columnDefinitions, string key)
        {
            return columnDefinitions.FirstOrDefault(c => c.EntityFieldName == key);
        }

        public CommandDefinition CreateAnUpdateCommandFromParameters(
            Dictionary<string, object> entityParameters,
            DapperTableDefinition tableDefinition,
            Dictionary<string, object> filterCriteria)
        {
            var updateScript = "Update " + tableDefinition.TableName + " Set" + Constants.StringEnter;

            var fieldsToUpdate = string.Empty;
            var dapperParameters = new DynamicParameters();

            foreach (var entityParameter in entityParameters)
            {
                var tableColumnDefinition = GetTableColumnDefinition(tableDefinition.ColumnDefinitions, entityParameter.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!fieldsToUpdate.IsEmpty())
                        fieldsToUpdate += Constants.StringComma + Constants.StringEnter;

                    fieldsToUpdate += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;

                    var objectValue = entityParameter.Value;

                    if (objectValue is IBaseEntity)
                        dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, (IBaseEntity)objectValue, size: tableColumnDefinition.Size);
                    else
                        dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, GetParameterValue(entityParameter.Key, entityParameter.Value), size: tableColumnDefinition.Size);
                }
            }

            updateScript += fieldsToUpdate + Constants.StringEnter + "Where" + Constants.StringEnter;

            var filterFieldNamesScript = string.Empty;

            foreach (var filterCriteriaPart in filterCriteria)
            {
                var tableColumnDefinition = GetTableColumnDefinition(tableDefinition.ColumnDefinitions, filterCriteriaPart.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!filterFieldNamesScript.IsEmpty())
                        filterFieldNamesScript += Constants.StringEnter + "And ";

                    if (filterCriteriaPart.Value is not null)
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;
                    else
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " is null";

                    var objectValue = filterCriteriaPart.Value;

                    if (objectValue is IBaseEntity)
                        dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, (IBaseEntity)objectValue, size: tableColumnDefinition.Size);
                    else
                        dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, GetParameterValue(filterCriteriaPart.Key, filterCriteriaPart.Value), size: tableColumnDefinition.Size);
                }
            }

            updateScript += filterFieldNamesScript + ";";

            var commandDefinition = new CommandDefinition(updateScript, dapperParameters);

            AddCommandDefinition(commandDefinition);

            return commandDefinition;
        }

        private static object GetParameterValue(string key, object value)
        {
            var entity = (value as IBaseEntity);

            if (entity is not null)
            {
                if (!key.Contains(Constants.CharFullStop)) // by default gets Id field 
                    return entity.Id;
                else
                {
                    string[] subProperties = key.Split(Constants.CharFullStop);

                    var entityType = entity.GetType();

                    var propertyName = subProperties[1];

                    var property = entityType.GetProperties()
                        .Where(p => p.Name == propertyName)
                        .FirstOrDefault();

                    if (property is not null)
                        return property.GetValue(entity, null);
                    else
                        return null;
                }
            }

            return value;
        }

        public CommandDefinition CreateAnUpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(IBaseEntity baseEntity)
        {

            return CreateAnUpdateCommandByEntityUpdatedPropertiesWithCriteria(
                baseEntity,
                new Dictionary<string, object>() { { nameof(baseEntity.Id), baseEntity.Id } });
        }

        public CommandDefinition CreateAnInsertCommandFromEntityProperties(IBaseEntity baseEntity)
        {
            var insertableProperties = baseEntity.GetPropertiesToPersist();

            var parentEntities = GetParentEntities(baseEntity.GetType());

            if (!parentEntities.IsNullOrEmpty())
                AddParentEntitiesAsEntityPropertiesToPersist(insertableProperties, parentEntities);

            var entityTableMapping = GetTableDefinitionByMappedType(baseEntity.GetType());

            if (entityTableMapping is null)
                throw new CommandExecutionException(Constants.ErrorMessages.EntityMappingError.Format(baseEntity.GetType().Name));

            return CreateAnInsertCommandFromParameters(insertableProperties, entityTableMapping.TableDefinition);
        }

        private void AddParentEntitiesAsEntityPropertiesToPersist(IDictionary<string, object> propertiesToPersist, IDictionary<string, IBaseEntity> parentEntities)
        {
            foreach (var parentEntity in parentEntities)
            {
                propertiesToPersist[parentEntity.Key] = parentEntity.Value;
            }
        }

        public CommandDefinition CreateAnUpdateCommandByEntityUpdatedPropertiesWithCriteria(
            IBaseEntity baseEntity,
            Dictionary<string, object> filterCriteria)
        {
            var updatedProperties = baseEntity.GetPropertiesToPersist();

            var parentEntities = GetParentEntities(baseEntity.GetType());

            if (!parentEntities.IsNullOrEmpty())
                AddParentEntitiesAsEntityPropertiesToPersist(updatedProperties, parentEntities);

            var entityTableMapping = GetTableDefinitionByMappedType(baseEntity.GetType());

            if (entityTableMapping is null)
                throw new CommandExecutionException(Constants.ErrorMessages.EntityMappingError.Format(baseEntity.GetType().Name));

            return CreateAnUpdateCommandFromParameters(updatedProperties, entityTableMapping.TableDefinition, filterCriteria);
        }

        private IDictionary<string, IBaseEntity> GetParentEntities(Type type)
        {
            var entityTableMapping = GetTableDefinitionByMappedType(type);

            if (entityTableMapping is null)
                return new Dictionary<string, IBaseEntity>();

            return entityTableMapping.ParentEntities;
        }

        public CommandDefinition CreateADeleteCommandWithEntityAndIdCriteria(
          IBaseEntity baseEntity)
        {
            var entityTableMapping = GetTableDefinitionByMappedType(baseEntity.GetType());

            if (entityTableMapping is null)
                throw new CommandExecutionException(Constants.ErrorMessages.EntityMappingError.Format(baseEntity.GetType().Name));

            return CreateADeleteCommandWithCriteria(
                new Dictionary<string, object>() { { nameof(baseEntity.Id), baseEntity.Id } },
                entityTableMapping.TableDefinition);
        }

        private EntityTableMapping GetTableDefinitionByMappedType(Type type)
        {
            var typeName = type.Name;

            if ((typeof(IProxy).IsAssignableFrom(type)) || typeName.EndsWith("Proxy"))
                typeName = typeName.Substring(0, typeName.Length - 5);// removes "Proxy" sufix

            _entityTableTypeMappings.TryGetValue(typeName, out var entityTableMapping);

            if (entityTableMapping is null)
                return null;

            return entityTableMapping;
        }

        public CommandDefinition CreateADeleteCommandWithCriteria(
        Dictionary<string, object> filterCriteria,
        DapperTableDefinition tableDefinition)
        {
            var deleteScript = "Delete From" + Constants.StringEnter + tableDefinition.TableName + Constants.StringEnter + "Where" + Constants.StringEnter;

            var filterFieldNamesScript = string.Empty;

            var parameters = new DynamicParameters();

            foreach (var filterCriteriaPart in filterCriteria)
            {
                var tableColumnDefinition = GetTableColumnDefinition(tableDefinition.ColumnDefinitions, filterCriteriaPart.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!filterFieldNamesScript.IsEmpty())
                        filterFieldNamesScript += Constants.StringEnter + "And ";

                    if (filterCriteriaPart.Value is not null)
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;
                    else
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " is null";

                    var objectValue = filterCriteriaPart.Value;

                    if (objectValue is IBaseEntity)
                        parameters.AddNullable(tableColumnDefinition.DbFieldName, (IBaseEntity)objectValue, size: tableColumnDefinition.Size);
                    else
                        parameters.AddNullable(tableColumnDefinition.DbFieldName, GetParameterValue(filterCriteriaPart.Key, filterCriteriaPart.Value), size: tableColumnDefinition.Size);
                }
            }

            deleteScript += filterFieldNamesScript + ";";

            var commandDefinition = new CommandDefinition(deleteScript, parameters);

            AddCommandDefinition(commandDefinition);

            return commandDefinition;
        }

        public IList<CommandDefinition> CreateCommandDefinitionByState<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : IBaseEntity
        {
            var commandDefinitions = new List<CommandDefinition>();

            foreach (var entity in entities)
            {
                var commandDefinition = CreateCommandDefinitionByState(entity);

                if (commandDefinition.HasValue)
                    commandDefinitions.Add(commandDefinition.Value);
            }

            return commandDefinitions;
        }

        public IList<CommandDefinition> CreateCommandDefinitionByState<TEntity>(IEntityList<TEntity> entities)
         where TEntity : IBaseEntity
        {
            var commandDefinitions = new List<CommandDefinition>();

            foreach (var entity in entities.DeletedItems)
            {
                var commandDefinition = CreateCommandDefinitionByState(entity);

                if (commandDefinition.HasValue)
                    commandDefinitions.Add(commandDefinition.Value);
            }

            var updateAndInsertCommands = CreateCommandDefinitionByState(entities.Items);

            commandDefinitions.AddRange(updateAndInsertCommands);

            return commandDefinitions;
        }

        public CommandDefinition? CreateCommandDefinitionByState<TEntity>(TEntity entity)
             where TEntity : IBaseEntity
        {
            CommandDefinition? commandDefinition = null;

            switch (entity.State)
            {
                case EntityState.New:
                    commandDefinition = CreateAnInsertCommandFromEntityProperties(entity);
                    break;
                case EntityState.Updated:
                    commandDefinition = CreateAnUpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(entity);
                    break;
                case EntityState.Deleted:
                    commandDefinition = CreateADeleteCommandWithEntityAndIdCriteria(entity);
                    break;
                default:
                    break;
            }

            return commandDefinition;
        }

        public virtual IList<CommandDefinition> CreateCommandDefinitions(Entity entity)
        {
            return [];
        }
    }
}