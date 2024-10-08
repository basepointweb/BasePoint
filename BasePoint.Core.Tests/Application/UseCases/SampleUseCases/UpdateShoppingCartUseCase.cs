using BasePoint.Core.Application.UseCases;
using BasePoint.Core.Extensions;
using BasePoint.Core.Tests.Application.SampleUseCasesDtos;
using BasePoint.Core.Tests.Domain.Repositories.SampleRepository;
using BasePoint.Core.UnitOfWork.Interfaces;

namespace BasePoint.Core.Tests.Application.UseCases.SampleUseCases
{
    public class UpdateShoppingCartUseCase : CommandUseCase<UpdateShoppingCartInput, ShoppingCartOutput>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        protected override string SaveChangesErrorMessage => "UpdateShoppingCartUseCase Error message";

        public UpdateShoppingCartUseCase(
            IShoppingCartRepository shoppingCartRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public override async Task<UseCaseOutput<ShoppingCartOutput>> InternalExecuteAsync(UpdateShoppingCartInput input)
        {
            var shoppingCart = _shoppingCartRepository.GetById(input.ShoppingCartId).Result
                .ThrowInvalidInputIfIsNotNull("")
                .ThrowInvalidInputIfMatches(shoppingCart => shoppingCart.Items.HasMissingEntities(input.Items.Select(x => x.ShoppingCartItemId)), "");

            _shoppingCartRepository.Persist(shoppingCart, UnitOfWork);

            await SaveChangesAsync();

            return CreateSuccessOutput(new ShoppingCartOutput());
        }
    }
}
