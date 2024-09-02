import { Inject } from '@angular/core';
import {
  httpDelete,
  httpGet,
  httpPost,
  httpPostFiles,
} from 'src/app/api/api-utils';
import { Pdf, PdfUploadRequest, Tag } from 'src/app/api/api.types';
import { environment } from '../../environments/environment';

@Inject({ providedIn: 'root' })
export default class PdfService {
  private _baseUrl: string = environment.apiUrl;

  async getPdfs(pdfTitle?: string): Promise<Pdf[]> {
    const queryString = pdfTitle ? `?title=${pdfTitle}` : '';
    const response = await httpGet<{ pdfs: Pdf[] }>(
      `${this._baseUrl}/Pdf${queryString}`
    );

    return response.pdfs;
  }

  async getPdf(id: number): Promise<Pdf> {
    const response = await httpGet<{ pdf: Pdf }>(`${this._baseUrl}/Pdf/${id}`);

    return response.pdf;
  }

  async createPdfRecord(pdf: PdfUploadRequest): Promise<Pdf> {
    const response = await httpPost<PdfUploadRequest, { pdf: Pdf }>(
      `${this._baseUrl}/Pdf`,
      pdf
    );

    return response.pdf;
  }

  async uploadPdf(pdfId: number, file: File): Promise<Pdf> {
    const formData = new FormData();
    formData.append('file', file);

    await httpPostFiles(`${this._baseUrl}/Pdf/${pdfId}`, formData);

    return this.getPdf(pdfId);
  }

  getPdfUrl(fileName: string): string {
    return `${this._baseUrl}/Pdf/file/${fileName}`;
  }

  async deletePdf(pdfId: number): Promise<void> {
    await httpDelete(`${this._baseUrl}/Pdf/${pdfId}`);
  }

  async getAllTags(): Promise<Tag[]> {
    const response = await httpGet<{ tags: Tag[] }>(
      `${this._baseUrl}/Pdf/tags`
    );

    return response.tags;
  }
}
