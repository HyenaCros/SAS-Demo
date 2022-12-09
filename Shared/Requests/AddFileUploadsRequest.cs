using Shared.Dtos;

namespace Shared;

public class AddFileUploadsRequest
{
    public List<BaseFileUpload> Files { get; set; }
    public bool IncludeFailures { get; set; }
}