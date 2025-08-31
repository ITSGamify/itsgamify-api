using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.Categories;
using its.gamify.domains.Entities;
using MediatR;

namespace its.gamify.api.Features.Categories.Commands
{
    public class CreateCategoryCommand : CategoryCreateModel, IRequest<Category>
    {
        class CommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, Category>
        {
            public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                bool checkDupName = (await unitOfWork.CategoryRepository.WhereAsync(x => x.Name.ToLower().Trim()== request.Name.ToLower().Trim())) != null;
                if (checkDupName) throw new BadRequestException("Tên danh mục đã tồn tại!");
                var category = unitOfWork.Mapper.Map<Category>(request);
                await unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
                await unitOfWork.SaveChangesAsync();
                return category;
            }
        }
    }
}
