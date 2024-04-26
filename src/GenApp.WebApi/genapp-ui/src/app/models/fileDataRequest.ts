export interface FileDataRequest {
    file: File;
    dbmsType: string;
    appName: string;
    dotnetSdkVersion: number;
    useDocker: boolean;
    connectionString?: string;
  }