import { Component } from '@angular/core';
import {
  DomSanitizer,
  SafeResourceUrl,
  Title,
} from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Pdf } from 'src/api/api.types';
import PdfService from 'src/services/pdf.service';

@Component({
  selector: 'app-pdf',
  templateUrl: './pdf.component.html',
  styleUrls: ['./pdf.component.scss'],
})
export class PdfComponent {
  pdf?: Pdf;
  pdfSrcSafe?: SafeResourceUrl;

  constructor(
    private _pdfService: PdfService,
    private _route: ActivatedRoute,
    private _titleService: Title,
    private _sanitizer: DomSanitizer
  ) {}

  async ngOnInit() {
    const id = Number(this._route.snapshot.paramMap.get('id'));
    const page = Number(this._route.snapshot.queryParamMap.get('page')) || 1;
    this.pdf = await this._pdfService.getPdf(id);
    if (this.pdf.hasFile) {
      this.setPdfSrc(this.pdf, page);
    }

    this.setTitle(this.pdf?.title || 'Default Title');
  }

  private setPdfSrc(pdf: Pdf, page: number) {
    const pdfSrc = this._pdfService.getPdfUrl(pdf.fileName) + `#page=${page}`;
    this.pdfSrcSafe = this._sanitizer.bypassSecurityTrustResourceUrl(pdfSrc);
  }

  private setTitle(title: string) {
    this._titleService.setTitle(title);
  }
}
