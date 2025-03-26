using BasePoint.Core.Domain.Cqrs;
using BasePoint.Core.UnitOfWork.Interfaces;

namespace BasePoint.Core.UnitOfWork
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        public IList<IEntityCommand> Commands { get; }

        public BaseUnitOfWork()
        {
            Commands = [];
        }

        public void AddComand(IEntityCommand command)
        {
            Commands.Add(command);
        }

        public void SetEntitiesPersistedState()
        {
            foreach (var command in Commands)
            {
                command.AffectedEntity.SetStateAsPersisted();
            }
        }

        public virtual async Task<bool> BeforeSaveAsync()
        {
            return await Task.FromResult(true);
        }

        public virtual async Task<bool> AfterSave(bool sucess)
        {
            return await Task.FromResult(sucess);
        }

        public virtual async Task AfterRollBackAsync()
        {
            await Task.FromResult(() => { });
        }

        public virtual async Task<UnitOfWorkResult> SaveChangesAsync()
        {
            bool sucess = await BeforeSaveAsync();
            var result = new UnitOfWorkResult(true, "Execute successfully.");

            if (!sucess)
                return new UnitOfWorkResult(false, "Error while preparing changes to be saved.");

            try
            {
                foreach (var command in Commands)
                {
                    sucess = await command.ExecuteAsync();

                    if (!sucess)
                        break;
                }
            }
            catch (Exception ex)
            {
                sucess = false;
                result = new UnitOfWorkResult(false, $"Error to saving changes: {ex.Message}. StackTrace: {ex.StackTrace}");
            }
            finally
            {
                if (sucess)
                {
                    sucess = await AfterSave(sucess);

                    if (!sucess)
                        result = new UnitOfWorkResult(false, "Error while executing post saving.");
                }

                if (sucess)
                    SetEntitiesPersistedState();

                Commands.Clear();
            }

            return result;
        }
        public abstract void Dispose();

        public async Task RollbackAsync()
        {
            await Task.FromResult(() => { });

            Commands.Clear();
        }
    }
}