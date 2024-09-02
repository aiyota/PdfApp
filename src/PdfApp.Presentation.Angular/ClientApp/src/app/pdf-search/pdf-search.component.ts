import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Pdf } from 'src/app/api/api.types';
import AuthService from 'src/app/services/auth.service';
import PdfService from 'src/app/services/pdf.service';
import { makeDebouncer } from 'src/app/utils';

@Component({
  selector: 'app-search-pdf',
  templateUrl: './pdf-search.component.html',
  styleUrls: ['./pdf-search.component.scss'],
})
export class SearchPdfComponent {
  private debouncer = makeDebouncer<Pdf[]>(500);
  pdfSearchStr: string = '';
  pdfs: Pdf[] = [];

  constructor(
    private _authService: AuthService,
    private _pdfService: PdfService
  ) {}

  async searchPdf(e: KeyboardEvent): Promise<void> {
    const target = e.target as HTMLInputElement;
    this.pdfSearchStr = target.value;
    this.pdfs = await this.debouncer(async () => {
      const pdfs = await this._pdfService.getPdfs(this.pdfSearchStr);
      return pdfs;
    });
  }

  async ngOnInit() {
    this.pdfs = await this._pdfService.getPdfs(this.pdfSearchStr);
  }
}
