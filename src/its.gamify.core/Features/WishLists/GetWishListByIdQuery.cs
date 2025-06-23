using its.gamify.domains.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace its.gamify.core.Features\WishLists.Queries
{
    public class GetWishListByIdQuery : IRequest<WishList>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetWishListByIdQuery, WishList>
        {
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }
            public async Task<WishList> Handle(GetWishListByIdQuery request, CancellationToken cancellationToken)
            {
                return await unitOfWork.WishListRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
            }
        }
    }
}
