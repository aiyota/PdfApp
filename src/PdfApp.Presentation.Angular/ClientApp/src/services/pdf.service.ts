import { Inject } from '@angular/core';
import { get } from 'src/api/api-utils';
import { Pdf } from 'src/api/api.types';

@Inject({ providedIn: 'root' })
export default class PdfService {
  async getPdfs(pdfTitle?: string): Promise<Pdf[]> {
    const queryString = pdfTitle ? `?title=${pdfTitle}` : '';

    const response = await get<{ pdfs: Pdf[] }>(
      `https://localhost:7017/api/Pdf${queryString}`
    );

    return response.pdfs;
  }

  async getPdf(id: number): Promise<Pdf> {
    const response = await get<{ pdf: Pdf }>(
      `https://localhost:7017/api/Pdf/${id}`
    );

    return response.pdf;
  }
}
