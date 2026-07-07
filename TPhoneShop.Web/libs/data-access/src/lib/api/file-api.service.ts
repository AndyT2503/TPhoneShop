import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { injectEnvironmentConfig } from '@tphone-shop.web/environment-config';
import { Observable } from 'rxjs';
import { UploadFileResponse } from '../models';

@Injectable({
  providedIn: 'root',
})
export class FileAPIService {
  private readonly httpClient = inject(HttpClient);
  private readonly envConfig = injectEnvironmentConfig();

  uploadFile(
    file: File,
    customFileName?: string,
  ): Observable<UploadFileResponse> {
    const formData = new FormData();
    formData.append('file', file);
    if (customFileName) {
      formData.append('customFileName', customFileName);
    }

    return this.httpClient.post<UploadFileResponse>(
      `${this.envConfig.fileService}/api/files`,
      formData,
    );
  }
}
