export interface FileUploadRecord {
  id: string;
  dateUploaded: string | Date;
  status: UploadStatus;
  fileName: string;
  type: ClaimType;
  source: FileSource;
  errors: ErrorRecord[];
  medicalClaims: FileClaimRecord[];
  hospitalClaims: FileClaimRecord[];
  dentalClaims: FileClaimRecord[];
  prescriptionClaims: FileClaimRecord[];
}

export interface ClaimRecord {
  claimType: ClaimType;
  claimNumber: number;
  claimDate: string;
  claimLineNumber: number;
  patientId: number;
  providerId: number;
  claimAmount: number;
  procedureDate?: string;
  procedureCode?: string;
  drugId?: number;
}

export interface FileClaimRecord extends ClaimRecord {
  id: string;
  dateCreated: string;
}

export interface ErrorRecord {
  id: string;
  errorMessage: string;
  lineNumber?: number;
  columnNumber?: number;
  line: string;
}

export interface ErrorLogRecord extends ErrorRecord {
  fileName: string;
  fileUploadId: string;
  dateCreated: string | Date;
}

export enum UploadStatus {
  Processing,
  Success,
  Failure,
  ContainsErrors
}

export enum ClaimType {
  Medical,
  Hospital,
  Dental,
  Prescription
}

export enum FileSource {
  FileServer,
  User
}