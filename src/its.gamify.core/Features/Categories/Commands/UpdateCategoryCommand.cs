using FluentValidation;
using its.gamify.core;
using its.gamify.core.GlobalExceptionHandling.Exceptions;
using its.gamify.core.Models.Categories;
using MediatR;

namespace its.gamify.core.Features.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public CategoryUpdateModel Model { get; set; } = new();
        class CommandValidate : AbstractValidator<UpdateCategoryCommand>
        {
            public CommandValidate()
            {
                RuleFor(x => x.Model.Name).NotEmpty().NotNull().WithMessage("Không để trống tên."); ;
            }
        }
        class CommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, bool>
        {
            private readonly IUnitOfWork unitOfWork = unitOfWork;

            public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = await unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
                if (category is not null)
                {
                    bool checkDupName = (await unitOfWork.CategoryRepository.WhereAsync(x => x.Name.ToLower().Trim() == request.Model.Name.ToLower().Trim() && x.Id != request.Id)) != null;
                    if (checkDupName) throw new BadRequestException("Tên danh mục đã tồn tại!");
                    unitOfWork.Mapper.Map(request.Model, category);
                    unitOfWork.CategoryRepository.Update(category);
                    return await unitOfWork.SaveChangesAsync();
                }
                else throw new BadRequestException("Không tìm thấy danh mục này!");

            }
        }
    }
}
