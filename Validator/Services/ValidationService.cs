using System.Globalization;
using System.Text;
using Shared;
using Validator.Models;

namespace Validator.Services;

public class ValidationService
{
    private readonly FileStorageService _fileStorageService;

    public ValidationService(FileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<ValidationResult> ValidateFileData(BaseFileData fileData)
    {
        var contents = await _fileStorageService.GetFileContents(fileData.FileName);
        var lines = contents.Split('\n').Where(x => !string.IsNullOrEmpty(x)).ToList();
        var errors = new List<ErrorRecord>();
        var records = new List<ClaimRecord>();
        for (var i = 0; i < lines.Count; i++)
        {
            ValidateLine(lines[i], i, fileData.FileType, records, errors);
        }
        
        RemoveDuplicates(records, errors);

        foreach (var error in errors)
        {
            error.FileName = fileData.FileName;
            if (error.LineNumber != null)
                error.Line = lines[error.LineNumber.Value];
        }

        return new ValidationResult()
        {
            Records = records,
            Errors = errors,
        };
    }

    private void ValidateLine(string line, int lineIndex, ClaimType claimType, List<ClaimRecord> records, List<ErrorRecord> errors)
    {
        var columns = line.Split('|');
        var validatedColumns = new List<object>();
        ErrorRecord error = null;
        for (var j = 0; j < columns.Length; j++)
        {
            try
            {
                var column = columns[j];
                if (column == null)
                    throw new Exception();
                switch (j)
                {
                    case 0:
                        validatedColumns.Add(column[0].ToFileType());
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
                        validatedColumns.Add(claimType == ClaimType.Prescription ? int.Parse(column) : DateTime.Parse(column));
                        break;
                    case 8:
                        validatedColumns.Add(column);
                        break;
                }
            }
            catch (ArgumentNullException e)
            {
                error = new ErrorRecord()
                {
                    ErrorMessage = "Null Column",
                    LineNumber = lineIndex,
                    ColumnNumber = j,
                };
                break;
            }
            catch (FormatException e)
            {
                error = new ErrorRecord()
                {
                    ErrorMessage = "Invalid Format",
                    LineNumber = lineIndex,
                    ColumnNumber = j,
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

public class ValidationResult
{
    public List<ErrorRecord> Errors { get; set; }
    public List<ClaimRecord> Records { get; set; }
}