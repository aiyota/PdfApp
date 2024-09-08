import { Component, HostListener, ViewChild } from '@angular/core';
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
  @ViewChild('pdfDialog') pdfDialog: any;

  pdf?: Pdf;
  pdfSrcSafe?: SafeResourceUrl;
  selectedFile: File | null = null;

  isFullScreen: boolean = false;

  constructor(
    private _pdfService: PdfService,
    private _route: ActivatedRoute,
    private _router: Router,
    private _titleService: Title,
    private _sanitizer: DomSanitizer
  ) {}

  async ngOnInit() {
    const id = Number(this._route.snapshot.paramMap.get('id'));
    let page = await this.getPage();
    this.pdf = await this._pdfService.getPdf(id);
    if (this.pdf.hasFile) {
      this.setPdfSrc(this.pdf, page);
    }

    this.setTitle(this.pdf?.title || 'Default Title');
  }

  saveProgress() {
    const page = Number(window.prompt('Enter your current page number'));
    if (!page || !this.pdf) {
      return;
    }

    this._pdfService.saveProgress(this.pdf.id, page);
  }

  async getPage() {
    let page = this.getCurrentPageFromUrl();
    if (!page) {
      page = await this.getCurrentPageFromApi();
    }

    return page;
  }

  getCurrentPageFromUrl(): number | null {
    const page = this._route.snapshot.queryParamMap.get('page');
    return page ? Number(page) : null;
  }

  async getCurrentPageFromApi(): Promise<number> {
    const id = Number(this._route.snapshot.paramMap.get('id'));
    const progresses = await this._pdfService.getProgresses(id);
    if (!progresses.length) {
      return 1;
    }

    return progresses[0].page;
  }

  ngOnDestroy() {
    // Ensure we exit full-screen mode when component is destroyed
    if (this.isFullScreen) {
      this.exitFullScreen();
    }
  }

  private setPdfSrc(pdf: Pdf, page: number) {
    const pdfSrc = this._pdfService.getPdfUrl(pdf.fileName) + `#page=${page}`;
    this.pdfSrcSafe = this._sanitizer.bypassSecurityTrustResourceUrl(pdfSrc);
  }

  @HostListener('document:keydown.escape', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (this.isFullScreen) {
      this.exitFullScreen();
    }
  }

  @HostListener('document:fullscreenchange', ['$event'])
  @HostListener('document:webkitfullscreenchange', ['$event'])
  @HostListener('document:mozfullscreenchange', ['$event'])
  @HostListener('document:MSFullscreenChange', ['$event'])
  onFullScreenChange() {
    this.isFullScreen = !!document.fullscreenElement;
  }

  toggleFullScreen() {
    this.isFullScreen = !this.isFullScreen;
    if (this.isFullScreen) {
      document.documentElement.requestFullscreen();
    } else {
      document.exitFullscreen();
    }
  }

  enterFullScreen() {
    const elem = document.documentElement;
    elem?.requestFullscreen();
  }

  exitFullScreen() {
    document?.exitFullscreen();
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

  openDialog() {
    this.pdfDialog.nativeElement.showModal();
  }

  closeDialog() {
    this.pdfDialog.nativeElement.close();
  }

  onUpdateComplete() {
    this.closeDialog();
    this.ngOnInit();
  }

  async setFavorite() {
    if (!this.pdf) return;
    this.pdf.isFavorite = !this.pdf.isFavorite;

    if (this.pdf.isFavorite) {
      await this._pdfService.addFavorite(this.pdf.id);
      return;
    }

    await this._pdfService.removeFavorite(this.pdf.id);
  }
}
