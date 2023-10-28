using Basket.Application.Commands;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers.CommandHandlers;

public class DeleteBasketByUsernameHandler : IRequestHandler<DeleteBasketByUsernameCommand, Unit>
{
    private readonly IBasketRepository _basketRepository;

    public DeleteBasketByUsernameHandler(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<Unit> Handle(DeleteBasketByUsernameCommand request, CancellationToken cancellationToken)
    {
        await _basketRepository.DeleteBasketAsync(request.Username);
        
        return Unit.Value;
    }
}
