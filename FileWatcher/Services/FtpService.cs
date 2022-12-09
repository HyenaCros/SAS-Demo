using System.Security.Cryptography;
using System.Text;
using Rebex.Net;
using Shared;
using Shared.Dtos;
using Shared.Services;

namespace FileWatcher.Services;

public class FtpService
{
    private readonly Sftp _ftpClient;
    private readonly AppSettings _appSettings;
    private readonly ILogger<FtpService> _logger;
    private readonly ValidationService _validationService;
    private readonly FileStorageService _fileStorageService;
    private readonly DataHandlerService _dataHandlerService;

    private List<SftpItem> _files;

    public FtpService(
        Sftp ftpClient,
        AppSettings appSettings,
        ILogger<FtpService> logger,
        ValidationService validationService,
        FileStorageService fileStorageService,
        DataHandlerService dataHandlerService)
    {
        _ftpClient = ftpClient;
        _appSettings = appSettings;
        _logger = logger;
        _validationService = validationService;
        _fileStorageService = fileStorageService;
        _dataHandlerService = dataHandlerService;
    }

    public async Task PollTriggerFiles()
    {
        var items = await _ftpClient.GetListAsync();

        _files = new();
        foreach (var item in items)
        {
            if (item.Type != SftpItemType.RegularFile)
                continue;
            _files.Add(item);
        }
        var rawTriggerFiles = _files.Where(x => x.Name.EndsWith(".trg")).ToList();
        if (rawTriggerFiles.Count == 0)
            return;
        var fileUploads = new List<BaseFileUpload>();
        foreach (var rawTriggerFile in rawTriggerFiles)
        {
            var fileUpload = new BaseFileUpload()
            {
                FileName = rawTriggerFile.Name,
                Source = FileSource.FileServer,
                DateUploaded = DateTime.Now,
            };
            fileUploads.Add(fileUpload);
            try
            {
                var dataFile = await ParseTriggerFile(rawTriggerFile);
                fileUpload.Type = dataFile.FileType;
                dataFile.Contents = await TryFetchFile(dataFile.FileName);
                ValidateChecksum(dataFile);
                ValidateRecordCount(dataFile);
            }
            catch (Exception e)
            {
                fileUpload.Errors.Add(new ErrorRecord()
                {
                    ErrorMessage = e.Message
                });
            }
        }

        var ids = await _dataHandlerService.AddFileUploads(new AddFileUploadsRequest()
        {
            Files = fileUploads
        });
        
        await _validationService.ValidateFiles(ids);
    }

    private async Task<FileData> ParseTriggerFile(SftpItem fileInfo)
    {
        var data = await TryFetchFile(fileInfo.Name);
        var columns = data.Split('|');
        if (columns.Length != 4)
            throw new Exception("Invalid Trigger File");
        return new FileData()
        {
            FileName = columns[0],
            FileType = columns[1][0].ToFileType(),
            RecordCount = int.Parse(columns[2]),
            Checksum = columns[3],
        };
    }

    private async Task<string> TryFetchFile(string fileName)
    {
        var fileInfo = _files.FirstOrDefault(x => x.Name == fileName);
        if (fileInfo == null)
            throw new Exception($"File Not Found: {fileName}");
        await using var stream = _ftpClient.GetDownloadStream(fileInfo.Path);
        using var reader = new StreamReader(stream);
        var data = await reader.ReadToEndAsync();
        await _fileStorageService.SaveFile(fileName, data);
        if (_appSettings.DeleteFiles)
            await _ftpClient.DeleteFileAsync(fileInfo.Path);
        return data;
    }

    private void ValidateChecksum(FileData fileData)
    {
        var bytes = Encoding.UTF8.GetBytes(fileData.Contents);
        var hash = SHA256.Create().ComputeHash(bytes);
        var builder = new StringBuilder();
        foreach (var b in hash)
        {
            builder.Append(b.ToString("X2"));
        }

        var computed = builder.ToString();
        if (computed != fileData.Checksum)
            throw new Exception($"Invalid Checksum: {fileData.Checksum}");
    }

    private void ValidateRecordCount(FileData fileData)
    {
        var lines = fileData.Contents.Split('\n').Count(x => !string.IsNullOrEmpty(x));
        if (lines != fileData.RecordCount)
            throw new Exception($"Invalid Record Count: Expected {fileData.RecordCount}, Found {lines}");
    }
}