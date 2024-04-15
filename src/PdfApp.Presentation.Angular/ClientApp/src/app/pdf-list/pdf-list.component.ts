import { Component, Input } from '@angular/core';
import { Pdf } from 'src/api/api.types';

@Component({
  selector: 'app-pdf-list',
  templateUrl: './pdf-list.component.html',
  styleUrls: ['./pdf-list.component.scss'],
})
export class PdfListComponent {
  @Input() pdfs: Pdf[] = [];
}
