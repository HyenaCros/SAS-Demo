import { BehaviorSubject, Observable } from 'rxjs';
import { ErrorLogRecord, FileUploadRecord } from '../types';

export class FileUploadService {
  private static _fileUploads: BehaviorSubject<Record<string, FileUploadRecord>>;
  private static _errorLogs: BehaviorSubject<Record<string, ErrorLogRecord>>;

  public static get FileUploads(): Observable<Record<string, FileUploadRecord>> {
    if (this._fileUploads == null) {
      this.UpdateFileUploads();
    }
    return this._fileUploads;
  }

  public static get ErrorLogs(): Observable<Record<string, ErrorLogRecord>> {
    if (this._errorLogs == null) {
      this.UpdateErrorLogs();
    }
    return this._errorLogs;
  }

  public static async Upload(file: File, type: string) {
    const data = new FormData();
    data.append('File', file, file.name);
    data.append('Type', type);
    const response = await fetch('/api/Uploads', {
      method: 'POST',
      body: data
    });
    const fileUpload = await response.json();
    if (this._fileUploads == null)
      return;
    fileUpload.dateCreated = new Date(fileUpload.dateCreated);
    this._fileUploads.value[fileUpload.id] = fileUpload;
    this._fileUploads.next(this._fileUploads.value);
  }

  public static async GetFileUpload(id: string) {
    const response = await fetch(`/api/Uploads/${id}`);
    if (!response.ok)
      return;
    var fileUpload = await response.json();
    return fileUpload as FileUploadRecord;
  }

  public static UpdateFileUploads() {
    if (this._fileUploads == null)
      this._fileUploads = new BehaviorSubject<Record<string, FileUploadRecord>>({});
    this.GetFileUploads().then(fileUploads => {
      fileUploads.forEach(fileUpload => {
        fileUpload.dateUploaded = new Date(fileUpload.dateUploaded);
        this._fileUploads.value[fileUpload.id] = fileUpload;
        if (this._errorLogs == null)
          return;
        fileUpload.errors.forEach(error => {
          const errorLog = error as ErrorLogRecord;
          errorLog.fileName = fileUpload.fileName;
          errorLog.fileUploadId = fileUpload.id;
          errorLog.dateCreated = fileUpload.dateUploaded;
          this._errorLogs.value[error.id] = errorLog;
        });
      });
      this._fileUploads.next(this._fileUploads.value);
      if (this._errorLogs)
        this._errorLogs.next(this._errorLogs.value);
    });
  }

  public static UpdateErrorLogs() {
    if (this._errorLogs == null)
      this._errorLogs = new BehaviorSubject<Record<string, ErrorLogRecord>>({});
    this.GetErrorLogs().then(errorLogs => {
      const state = this._errorLogs.value;
      errorLogs.forEach(errorLog => {
        errorLog.dateCreated = new Date(errorLog.dateCreated);
        state[errorLog.id] = errorLog;
      });
      this._errorLogs.next(state);
    });
  }

  private static async GetFileUploads() {
    const response = await fetch('/api/Uploads');
    var fileUploads = await response.json();
    return fileUploads as FileUploadRecord[];
  }

  public static async GetErrorLogs() {
    const response = await fetch('/api/Uploads/Errors');
    var errors = await response.json();
    return errors as ErrorLogRecord[];
  }
}