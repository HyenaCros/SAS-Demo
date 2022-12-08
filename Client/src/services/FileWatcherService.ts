import config from '../../appsettings.json';
export class FileWatcherService {
    public static async Start() {
        await fetch(`${config.API}/api/Polling/Start`, {
            method: 'POST'
        });
    }
    
    public static async Stop() {
        await fetch(`${config.API}/api/Polling/Stop`, {
            method: 'POST'
        });
    }
    
    public static async GetStatus() {
        const response = await fetch(`${config.API}/api/Polling`);
        const { status } = await response.json();
        return status as boolean;
    }
}