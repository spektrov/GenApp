import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from '../../environments/enviroment';
import { FileDataRequest } from '../models/fileDataRequest';
import { Observable, of } from 'rxjs';
import { map, switchMap, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class GenappService {

  constructor(private httpClient: HttpClient) { }

  generateWebApp(request : FileDataRequest) : Observable<Blob> {
    let url = `${enviroment.apiUrl}/api/genapp/file`;

    let formData = this.toFormData(request);

    return this.httpClient.post(url, formData, { responseType: 'blob' as 'json' }).pipe(
      map((res: any) => {
          return new Blob([res], { type: 'application/zip' });
      })
    );
  }

  downloadFile(blob: Blob, fileName: string): void {
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName + '.zip';
    link.click();

    window.URL.revokeObjectURL(url);
}

  private toFormData(request: FileDataRequest): FormData {
    const formData = new FormData();

    formData.append('file', request.file);
    formData.append('dbmsType', request.dbmsType);
    formData.append('appName', request.appName);
    formData.append('dotnetSdkVersion', request.dotnetSdkVersion.toString());
    formData.append('useDocker', request.useDocker.toString());
    if(request.connectionString) {
        formData.append('connectionString', request.connectionString);
    }
      
    return formData;
  }
}
