import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Pdf } from 'src/app/api/api.types';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-pdf-list',
  templateUrl: './pdf-list.component.html',
  styleUrls: ['./pdf-list.component.scss'],
})
export class PdfListComponent {
  @Output() setFavoriteEvent = new EventEmitter<Pdf>();
  @Input() isLoading: boolean = false;
  @Input() pdfs: Pdf[] = [];
  baseUrl: string = environment.apiUrl;

  setFavorite(pdf: Pdf): void {
    pdf.isFavorite = !pdf.isFavorite;
    this.setFavoriteEvent.emit(pdf);
  }
}
