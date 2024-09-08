import { Inject } from '@angular/core';
import {
  httpDelete,
  httpGet,
  httpPatch,
  httpPost,
  httpPostFiles,
} from 'src/app/api/api-utils';
import { Pdf, PdfUploadRequest, Progress, Tag } from 'src/app/api/api.types';
import { environment } from '../../environments/environment';

@Inject({ providedIn: 'root' })
export default class PdfService {
  private _baseUrl: string = environment.apiUrl;

  async getPdfs(pdfTitle?: string, tags?: string[]): Promise<Pdf[]> {
    let queryString = pdfTitle ? `?title=${encodeURIComponent(pdfTitle)}` : '';

    if (tags && tags.length > 0) {
      let tagsQueryString = queryString ? '&' : '?';
      tagsQueryString += 'tags=' + encodeURIComponent(tags.join(','));
      queryString += tagsQueryString;
    }

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

    return response!.pdf;
  }

  async uploadPdf(pdfId: number, file: File): Promise<Pdf> {
    const formData = new FormData();
    formData.append('file', file);

    await httpPostFiles(`${this._baseUrl}/Pdf/${pdfId}`, formData);

    return this.getPdf(pdfId);
  }

  async updatePdf(pdfId: number, pdf: PdfUploadRequest): Promise<Pdf> {
    const response = await httpPatch<PdfUploadRequest, { pdf: Pdf }>(
      `${this._baseUrl}/Pdf/${pdfId}`,
      pdf
    );

    return response.pdf;
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

  async saveProgress(pdfId: number, page: number): Promise<void> {
    await httpPost(`${this._baseUrl}/Pdf/progress/${pdfId}`, {
      page,
    });
  }

  async getProgresses(pdfId: number): Promise<Progress[]> {
    const response = await httpGet<{ progresses: Progress[] }>(
      `${this._baseUrl}/Pdf/progress/${pdfId}`
    );

    return response.progresses;
  }

  async getFavoritePdfs(pdfTitle?: string, tags?: string[]): Promise<Pdf[]> {
    let queryString = pdfTitle ? `?title=${encodeURIComponent(pdfTitle)}` : '';

    if (tags && tags.length > 0) {
      let tagsQueryString = queryString ? '&' : '?';
      tagsQueryString += 'tags=' + encodeURIComponent(tags.join(','));
      queryString += tagsQueryString;
    }

    const response = await httpGet<{ pdfs: Pdf[] }>(
      `${this._baseUrl}/Pdf/favorites${queryString}`
    );

    return response.pdfs;
  }

  async addFavorite(pdfId: number): Promise<void> {
    await httpPost(`${this._baseUrl}/Pdf/favorites/${pdfId}`, null, true);
  }

  async removeFavorite(pdfId: number): Promise<void> {
    await httpDelete(`${this._baseUrl}/Pdf/favorites/${pdfId}`);
  }

  getPdfDownloadLink(fileName: string): string {
    return `${this._baseUrl}/Pdf/file/${fileName}`;
  }
}
