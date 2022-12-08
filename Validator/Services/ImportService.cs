using Shared;

namespace Validator.Services;

public class ImportService
{
    private readonly ValidationService _validationService;
    private readonly DataHandlerService _dataHandlerService;

    public ImportService(ValidationService validationService, DataHandlerService dataHandlerService)
    {
        _validationService = validationService;
        _dataHandlerService = dataHandlerService;
    }
    public async Task ImportFileData(BaseFileData fileData)
    {
        try
        {
            var results = await _validationService.ValidateFileData(fileData);
            if (results.Errors.Count > 0)
                await _dataHandlerService.AddErrorRecords(results.Errors);
            if (results.Records.Count > 0)
                await _dataHandlerService.AddClaimRecords(results.Records);
        }
        catch
        {
            
        }
    }
}