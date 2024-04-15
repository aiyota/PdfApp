import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
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

  constructor(
    private _pdfService: PdfService,
    private _route: ActivatedRoute,
    private _titleService: Title
  ) {}

  async ngOnInit() {
    const id = Number(this._route.snapshot.paramMap.get('id'));
    this.pdf = await this._pdfService.getPdf(id);

    this.setTitle(this.pdf?.title || 'Default Title');
  }

  private setTitle(title: string) {
    this._titleService.setTitle(title || 'Default Title');
  }
}
