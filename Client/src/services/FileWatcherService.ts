export class FileWatcherService {
    public static async Start() {
        await fetch('/api/Polling/Start', {
            method: 'POST'
        });
    }
    
    public static async Stop() {
        await fetch('/api/Polling/Stop', {
            method: 'POST'
        });
    }
    
    public static async GetStatus() {
        const response = await fetch('/api/Polling');
        const { status } = await response.json();
        return status as boolean;
    }
}