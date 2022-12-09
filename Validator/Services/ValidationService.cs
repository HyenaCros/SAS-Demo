using System.Globalization;
using System.Text;
using Shared;
using Validator.Models;

namespace Validator.Services;

public class ValidationService
{
    private const int MAX_COLUMNS = 9;
    private const int MIN_COLUMNS = 8;
    private readonly FileStorageService _fileStorageService;
    private readonly DataHandlerService _dataHandlerService;
    private readonly AppSettings _appSettings;

    public ValidationService(FileStorageService fileStorageService, DataHandlerService dataHandlerService,
        AppSettings appSettings)
    {
        _fileStorageService = fileStorageService;
        _dataHandlerService = dataHandlerService;
        _appSettings = appSettings;
    }

    public async Task ValidateFileData(Guid id)
    {
        var fileUpload = await _dataHandlerService.GetFileUpload(id);
        var contents = await _fileStorageService.GetFileContents(fileUpload.FileName);
        var lines = contents.Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToList();
        var errors = new List<ErrorRecord>();
        var records = new List<ClaimRecord>();
        for (var i = 0; i < lines.Count; i++)
        {
            ValidateLine(lines[i], i, fileUpload.Type, records, errors);
        }

        RemoveDuplicates(records, errors);

        await _dataHandlerService.UpdateFileUpload(new ValidationResult()
        {
            Id = id,
            ClaimRecords = records,
            ErrorRecords = errors,
        });
    }

    private void ValidateLine(string line, int lineIndex, ClaimType claimType, List<ClaimRecord> records,
        List<ErrorRecord> errors)
    {
        var columns = line.Split('|');
        var validatedColumns = new List<object>();
        if ((claimType == ClaimType.Prescription && columns.Length < MIN_COLUMNS) ||
            columns.Length < MIN_COLUMNS + 1)
        {
            errors.Add(new ErrorRecord()
            {
                ErrorMessage = "Too Few Columns",
                LineNumber = lineIndex,
                Line = line
            });
            return;
        }
        ErrorRecord error = null;
        for (var j = 0; j < MAX_COLUMNS; j++)
        {
            try
            {
                var column = columns[j];
                if (string.IsNullOrEmpty(column))
                    throw new Exception("Null Column");
                switch (j)
                {
                    case 0:
                        var type = column[0].ToFileType();
                        if (!_appSettings.IgnoreMismatchedTypes && type != claimType)
                            throw new Exception("Line Type Mismatch");
                        validatedColumns.Add(type);
                        break;
                    case 1:
                    case 3:
                    case 4:
                    case 5:
                        validatedColumns.Add(int.Parse(column));
                        break;
                    case 6:
                        validatedColumns.Add(decimal.Parse(column));
                        break;
                    case 2:
                        validatedColumns.Add(DateTime.Parse(column));
                        break;
                    case 7:
                        validatedColumns.Add((ClaimType) validatedColumns[0] == ClaimType.Prescription
                            ? int.Parse(column)
                            : DateTime.Parse(column));
                        break;
                    case 8:
                        if ((ClaimType) validatedColumns[0] == ClaimType.Prescription)
                            break;
                        validatedColumns.Add(column);
                        break;
                }
            }
            catch (FormatException)
            {
                error = new ErrorRecord()
                {
                    ErrorMessage = "Invalid Format",
                    LineNumber = lineIndex,
                    ColumnNumber = j,
                    Line = line
                };
                break;
            }
            catch (Exception e)
            {
                error = new ErrorRecord()
                {
                    ErrorMessage = e.Message,
                    LineNumber = lineIndex,
                    ColumnNumber = j,
                    Line = line
                };
                break;
            }
        }

        if (error != null)
        {
            errors.Add(error);
            return;
        }

        records.Add(ClaimRecord.Create(validatedColumns));
    }

    private void RemoveDuplicates(List<ClaimRecord> records, List<ErrorRecord> errors)
    {
        var hashMap = new Dictionary<string, List<int>>();
        for (var i = 0; i < records.Count; i++)
        {
            var hash = records[i].Hash();
            if (!hashMap.ContainsKey(hash))
                hashMap[hash] = new List<int>();
            hashMap[hash].Add(i);
        }

        var duplicateErrors = hashMap
            .Where(x => x.Value.Count > 1)
            .SelectMany(x =>
            {
                var duplicates = new List<ErrorRecord>();
                for (var i = 1; i < x.Value.Count; i++)
                {
                    duplicates.Add(new ErrorRecord()
                    {
                        LineNumber = x.Value[i],
                        ErrorMessage = "Duplicate Record",
                    });
                    records.RemoveAt(x.Value[i]);
                }

                return duplicates;
            });

        errors.AddRange(duplicateErrors);
    }
}