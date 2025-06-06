﻿using BasePoint.Core.Domain.Entities.Interfaces;
using BasePoint.Core.Exceptions;
using BasePoint.Core.UnitOfWork.Interfaces;

namespace BasePoint.Core.Application.UseCases
{
    public abstract class CommandUseCase<Input, Output>(IUnitOfWork unitOfWork) : BaseUseCase<Input, Output>
    {
        public delegate Task<Entity> QueryMethod<InType, Entity>(InType inputValue);
        protected IUnitOfWork UnitOfWork { get; } = unitOfWork;
        protected abstract string SaveChangesErrorMessage { get; }

        protected static async Task ThrowsIfEntityAlreadyExistsAsync<InType, Entity, ExistsExceptionType>(
            QueryMethod<InType, Entity> queryMethod, InType inputValue, string errorMessage)
            where Entity : IBaseEntity
            where ExistsExceptionType : BaseException
        {
            Entity entity = await queryMethod.Invoke(inputValue);

            if (entity is not null)
                throw (ExistsExceptionType)Activator.CreateInstance(typeof(ExistsExceptionType), [errorMessage]);
        }

        protected static void ThrowsIfEntityDoesNotExists<InType, Entity, ExistsExceptionType>(
            QueryMethod<InType, Entity> queryMethod, InType inputValue, string errorMessage, out Entity entity)
            where Entity : IBaseEntity
            where ExistsExceptionType : BaseException
        {
            entity = (queryMethod.Invoke(inputValue)).Result;

            if (entity is null)
                throw (ExistsExceptionType)Activator.CreateInstance(typeof(ExistsExceptionType), [errorMessage]);
        }

        protected static void ThrowsResourceNotFoundIfEntityDoesNotExists<InType, Entity>(
            QueryMethod<InType, Entity> queryMethod, InType inValue, string errorMessage, out Entity entity)
            where Entity : IBaseEntity
        {
            ThrowsIfEntityDoesNotExists<InType, Entity, ResourceNotFoundException>(queryMethod, inValue, errorMessage, out entity);
        }

        protected static async Task ThrowsInvalidInputIfEntityExistsAsync<InType, Entity>(
            QueryMethod<InType, Entity> queryMethod, InType inputValue, string errorMessage)
            where Entity : IBaseEntity
        {
            await ThrowsIfEntityAlreadyExistsAsync<InType, Entity, InvalidInputException>(queryMethod, inputValue, errorMessage);
        }

        protected virtual async Task SaveChangesAsync()
        {
            var result = await UnitOfWork.SaveChangesAsync();

            if (!result.Success)
            {
                await UnitOfWork.RollbackAsync();

                throw new ExecutionErrorException(result.Message);
            }
        }
    }
}
