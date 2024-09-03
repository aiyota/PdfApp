import { Component, Input } from '@angular/core';
import { Pdf } from 'src/app/api/api.types';
import PdfService from '../services/pdf.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-pdf-list',
  templateUrl: './pdf-list.component.html',
  styleUrls: ['./pdf-list.component.scss'],
})
export class PdfListComponent {
  @Input() pdfs: Pdf[] = [];
  baseUrl: string = environment.apiUrl;

  makePdfUrl(fileName: string): string {
    return `${this.baseUrl}/Pdf/file/${fileName}`;
  }
}
