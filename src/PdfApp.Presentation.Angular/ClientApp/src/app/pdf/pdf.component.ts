import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  DomSanitizer,
  SafeResourceUrl,
  Title,
} from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Pdf } from 'src/app/api/api.types';
import PdfService from 'src/app/services/pdf.service';

@Component({
  selector: 'app-pdf',
  templateUrl: './pdf.component.html',
  styleUrls: ['./pdf.component.scss'],
})
export class PdfComponent {
  pdf?: Pdf;
  pdfSrcSafe?: SafeResourceUrl;
  selectedFile: File | null = null;

  constructor(
    private _pdfService: PdfService,
    private _route: ActivatedRoute,
    private _router: Router,
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

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    const file: File = (target.files as FileList)[0];
    this.selectedFile = file;
  }

  async uploadPdf(event: Event) {
    event.preventDefault();
    if (!this.selectedFile || !this.pdf) {
      return;
    }

    await this._pdfService.uploadPdf(this.pdf.id, this.selectedFile);
    const pdf = await this._pdfService.getPdf(this.pdf.id);
    if (pdf) {
      this.pdf = pdf;
      this.setPdfSrc(pdf, 1);
      this.setTitle(pdf.title);
      return;
    }

    alert('Failed to upload PDF');
  }

  deletePdf() {
    const confirmed = window.confirm(
      'Are you sure you want to delete this PDF?'
    );

    if (!confirmed || !this.pdf) {
      return;
    }

    this._pdfService.deletePdf(this.pdf.id);

    this._router.navigate(['/']);
  }
}
