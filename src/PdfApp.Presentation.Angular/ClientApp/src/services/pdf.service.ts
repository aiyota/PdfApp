import { Inject } from '@angular/core';
import { get } from 'src/api/api-utils';
import { Pdf } from 'src/api/api.types';
import { environment } from '../environments/environment';

@Inject({ providedIn: 'root' })
export default class PdfService {
  private _baseUrl: string = environment.apiUrl;

  async getPdfs(pdfTitle?: string): Promise<Pdf[]> {
    const queryString = pdfTitle ? `?title=${pdfTitle}` : '';
    const response = await get<{ pdfs: Pdf[] }>(
      `${this._baseUrl}/Pdf${queryString}`
    );

    return response.pdfs;
  }

  async getPdf(id: number): Promise<Pdf> {
    const response = await get<{ pdf: Pdf }>(`${this._baseUrl}/Pdf/${id}`);

    return response.pdf;
  }

  getPdfUrl(fileName: string): string {
    return `${this._baseUrl}/Pdf/file/${fileName}`;
  }
}
