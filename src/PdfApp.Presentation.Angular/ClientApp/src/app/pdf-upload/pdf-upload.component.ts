import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import PdfService from '../services/pdf.service';
import { Pdf, Tag, TagRequest } from '../api/api.types';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pdf-upload',
  templateUrl: './pdf-upload.component.html',
  styleUrls: ['./pdf-upload.component.scss'],
})
export class PdfUploadComponent implements OnInit {
  pdfUploadFile: File | null = null;
  existingTags: string[] = [];
  displayNewTags = false;
  @Input() pdf: Pdf | null = null;
  @Output() updateComplete = new EventEmitter<void>();
  isLoading = false;

  uploadForm = new FormGroup({
    title: new FormControl('', [Validators.required]),
    description: new FormControl('', [Validators.required]),
    author: new FormControl('', [Validators.required]),
    totalPages: new FormControl(0, [Validators.min(1)]),
    tags: new FormControl(''),
    pdfUpload: new FormControl<File | null>(null, [Validators.required]),
  });

  constructor(private _pdfService: PdfService, private _router: Router) {}

  ngOnInit(): void {
    this.setPdfUploadValidator();

    this._pdfService.getAllTags().then((tags) => {
      this.existingTags = tags.map((tag) => tag.name.toLowerCase());
    });

    if (this.pdf) {
      this.uploadForm.controls.title.setValue(this.pdf.title);
      this.uploadForm.controls.description.setValue(this.pdf.description);
      this.uploadForm.controls.author.setValue(this.pdf.author);
      this.uploadForm.controls.totalPages.setValue(this.pdf.totalPages);
      this.uploadForm.controls.tags.setValue(
        this.pdf.tags.map((tag) => tag.name).join(', ')
      );
    }
  }

  // Dynamically set validator based on whether the pdf is provided
  setPdfUploadValidator(): void {
    if (this.pdf) {
      // If a PDF is provided, remove the 'required' validator for pdfUpload
      this.uploadForm.get('pdfUpload')?.clearValidators();
    } else {
      // If no PDF is provided, add the 'required' validator for pdfUpload
      this.uploadForm.get('pdfUpload')?.setValidators([Validators.required]);
    }
    // Update the validity of the control after changing validators
    this.uploadForm.get('pdfUpload')?.updateValueAndValidity();
  }

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
    if (this.pdf) return false;

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
      title: this.uploadForm.controls.title.value!.trim(),
      description: this.uploadForm.controls.description.value!.trim(),
      author: this.uploadForm.controls.author.value!.trim(),
      totalPages: this.uploadForm.controls.totalPages.value!,
      tags: this.getTagsToUpload(),
    };

    if (this.pdf) {
      this.isLoading = true;
      await this._pdfService.updatePdf(this.pdf.id, pdfUploadRequest);
      if (this.pdfUploadFile)
        await this._pdfService.uploadPdf(this.pdf.id, this.pdfUploadFile);
      this.isLoading = false;

      this.pdfUploadFile = null;
      this.uploadForm.controls.pdfUpload.setValue(null);
      this.updateComplete.emit();

      return;
    }

    this.isLoading = true;
    const newPdf = await this._pdfService.createPdfRecord(pdfUploadRequest);
    await this._pdfService.uploadPdf(newPdf.id, this.pdfUploadFile!);
    this.isLoading = false;
    this._router.navigate(['/']);
  }

  get newTags() {
    const uploadedTags = this.getTagsToUpload().map((tag) =>
      tag.name.toLowerCase()
    );

    const newTags = uploadedTags
      .filter((tag) => !this.existingTags.includes(tag) && tag !== '')
      .map((tag) => tag.trim());

    return newTags;
  }

  tagsOnTyping() {
    if (this.newTags.length > 0) this.displayNewTags = true;
    else this.displayNewTags = false;
  }

  private getTagsToUpload(): TagRequest[] {
    return (
      this.uploadForm.controls.tags.value
        ?.split(',')
        .map((tag: string) => ({ name: tag.trim() })) ?? []
    );
  }
}
