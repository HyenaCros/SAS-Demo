import config from '../../appsettings.json';
import { ErrorRecord } from '../models/ErrorRecord';

export class FileUploadService {
    public static async Upload(file: File, type: string) {
        const data = new FormData();
        data.append('File', file, file.name);
        data.append('Type', type);
        await fetch(`${config.API}/api/Upload`, {
            method: 'POST',
            body: data
        });
    }
    public static async GetErrors() {
        const response = await fetch(`${config.API}/api/Upload/Errors`);
        var errors = await response.json();
        return errors as ErrorRecord[];
    }
}