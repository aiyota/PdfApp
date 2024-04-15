import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Pdf } from 'src/api/api.types';
import AuthService from 'src/services/auth.service';
import PdfService from 'src/services/pdf.service';
import { makeDebouncer } from 'src/utils';

@Component({
  selector: 'app-search-pdf',
  templateUrl: './search-pdf.component.html',
  styleUrls: ['./search-pdf.component.scss'],
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
      console.log({ pdfs });
      return pdfs;
    });

    console.log(this.pdfs);
  }

  async ngOnInit() {
    this.pdfs = await this._pdfService.getPdfs(this.pdfSearchStr);
  }
}
