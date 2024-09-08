import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { Pdf, Tag } from 'src/app/api/api.types';
import AuthService from 'src/app/services/auth.service';
import PdfService from 'src/app/services/pdf.service';
import { deepClone, makeDebouncer } from 'src/app/utils';

@Component({
  selector: 'app-search-pdf',
  templateUrl: './pdf-search.component.html',
  styleUrls: ['./pdf-search.component.scss'],
})
export class SearchPdfComponent implements OnInit {
  private debouncer = makeDebouncer<void>(500);
  pdfSearchStr: string = '';
  pdfs: Pdf[] = [];
  availableTags: Tag[] = [];
  filteredTags: Tag[] = [];
  selectedTags: Tag[] = [];
  isFavoriteSearch = false;
  isLoading = false;

  constructor(private _titleService: Title, private _pdfService: PdfService) {}

  async ngOnInit() {
    this.isLoading = true;
    this.pdfs = await this._pdfService.getPdfs(this.pdfSearchStr);
    this.availableTags = await this._pdfService.getAllTags();
    this.isLoading = false;

    this.filteredTags = deepClone(this.availableTags);
    this._titleService.setTitle('Search PDFs');
  }

  get showClearTagButton(): boolean {
    return this.selectedTags.length > 0;
  }

  async searchPdf(e: KeyboardEvent): Promise<void> {
    const target = e.target as HTMLInputElement;
    this.pdfSearchStr = target.value;
    await this.debouncer(async () => {
      await this.setPdfs();
    });
  }

  async tagSelected(event: KeyboardEvent): Promise<void> {
    if (event.key !== 'Enter') return;
    event.preventDefault();
    const select = event.target as HTMLSelectElement;
    const selectedTag = this.availableTags.find(
      (tag) => tag.id === Number(select.value)
    );

    if (!selectedTag) {
      return;
    }

    this.selectedTags.push(selectedTag);
    this.filteredTags = this.filteredTags.filter(
      (tag) => tag.id !== selectedTag.id
    );

    await this.setPdfs();
  }

  async removeTag(tag: Tag): Promise<void> {
    this.selectedTags = this.selectedTags.filter((t) => t.id !== tag.id);
    this.filteredTags.push(tag);

    await this.setPdfs();
  }

  async clearTags() {
    this.selectedTags = [];
    this.filteredTags = deepClone(this.availableTags);

    await this.setPdfs();
  }

  async toggleFavoriteSearch() {
    this.isFavoriteSearch = !this.isFavoriteSearch;

    await this.setPdfs();
  }

  async setPdfs() {
    this.isLoading = true;
    if (this.isFavoriteSearch) {
      this.pdfs = await this._pdfService.getFavoritePdfs(
        this.pdfSearchStr,
        this.selectedTags.map((tag) => tag.name)
      );
    } else {
      this.pdfs = await this._pdfService.getPdfs(
        this.pdfSearchStr,
        this.selectedTags.map((tag) => tag.name)
      );
    }

    this.isLoading = false;
  }

  async onSetFavorite(pdf: Pdf): Promise<void> {
    if (pdf.isFavorite) {
      await this._pdfService.addFavorite(pdf.id);
      return;
    }

    await this._pdfService.removeFavorite(pdf.id);

    await this.setPdfs();
  }

  setSearchStr(event: Event) {
    const target = event.target as HTMLInputElement;
    this.pdfSearchStr = target.value;
  }

  async resetSearch() {
    this.pdfSearchStr = '';
    this.selectedTags = [];
    this.filteredTags = deepClone(this.availableTags);
    this.isFavoriteSearch = false;

    await this.setPdfs();
  }
}
