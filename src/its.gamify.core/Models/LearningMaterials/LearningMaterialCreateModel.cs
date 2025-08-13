using its.gamify.core.Models.Files;
using its.gamify.domains.Enums;

namespace its.gamify.core.Models.LearningMaterials
{
    public class LearningMaterialCreateModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = LearningMaterialType.UNDEFINED.ToString();// e.g., Video, Article, Quiz
        public FileUploadRequestModel File { get; set; }

    }
}
