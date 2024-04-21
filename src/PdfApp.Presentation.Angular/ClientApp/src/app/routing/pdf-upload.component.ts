import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import PdfService from '../services/pdf.service';

@Component({
  selector: 'app-pdf-upload',
  templateUrl: './pdf-upload.component.html',
  styleUrls: ['./pdf-upload.component.scss'],
})
export class PdfUploadComponent {
  pdfUploadFile: File | null = null;

  uploadForm = new FormGroup({
    title: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    author: new FormControl('', [Validators.required]),
    totalPages: new FormControl(0, [Validators.min(1)]),
    // tags: new FormControl([]), TODO
    pdfUpload: new FormControl<File | null>(null, [Validators.required]),
  });

  constructor(private _pdfService: PdfService) {}

  get titleIsInvalid() {
    return this.getValidStatus('title', ['required']);
  }

  get descriptionIsInvalid() {
    return this.getValidStatus('description', ['required']);
  }

  get authorIsInvalid() {
    return this.getValidStatus('author', ['required']);
  }

  get totalPagesAreInvalid() {
    return this.getValidStatus('totalPages', ['min']);
  }

  get pdfUploadIsInvalid() {
    return this.getValidStatus('pdfUpload', ['required']);
  }

  private getValidStatus(
    controlName: keyof typeof this.uploadForm.controls,
    errorNames: string[]
  ) {
    const control = this.uploadForm.controls[controlName];

    if (!control.touched || !control.dirty) {
      return false;
    }

    return errorNames.every((error) => Boolean(control.errors?.[error]));
  }

  onFileChange(event: Event) {
    const target = event.target as HTMLInputElement;
    const file: File = (target.files as FileList)[0];
    this.pdfUploadFile = file;
  }

  async uploadPdf() {
    const pdfUploadRequest = {
      id: 0,
      title: this.uploadForm.controls.title.value?.trim(),
      description: this.uploadForm.controls.description.value?.trim(),
      author: this.uploadForm.controls.author.value?.trim(),
      totalPages: this.uploadForm.controls.totalPages.value,
    };

    // get id
    const newPdf = await this._pdfService.createPdfRecord(pdfUploadRequest);

    // submit file upload

    await this._pdfService.uploadPdf(newPdf.id, this.pdfUploadFile!);
  }
}
