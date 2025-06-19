using its.gamify.core.IntegrationServices.Interfaces;
using its.gamify.core.Models.Files;
using MediatR;

namespace its.gamify.api.Features.Files.Commands
{
    public class UploadFileCommand : FileUploadRequestModel, IRequest<FileUploadResponseModel>
    {
        class CommandHandler : IRequestHandler<UploadFileCommand, FileUploadResponseModel>
        {
            private readonly IFirebaseService firebaseService;
            public CommandHandler(IFirebaseService firebaseService)
            {
                this.firebaseService = firebaseService;
            }
            public async Task<FileUploadResponseModel> Handle(UploadFileCommand request, CancellationToken cancellationToken)
            {
                var res = await firebaseService.UploadFileAsync(request.File, string.IsNullOrEmpty(request.Directory) ? "its-gamify/storage" : request.Directory);
                if (!string.IsNullOrEmpty(res.url))
                {
                    return new FileUploadResponseModel()
                    {
                        ContentType = "",
                        FileName = res.fileName,
                        Url = res.url
                    };
                }
                throw new InvalidOperationException("Upload File Failed");
            }
        }

    }
}
