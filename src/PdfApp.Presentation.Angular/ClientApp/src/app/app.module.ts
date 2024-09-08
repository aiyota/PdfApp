import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SearchPdfComponent } from './pdf-search/pdf-search.component';
import { LoginComponent } from './login/login.component';
import AuthService from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import PdfService from 'src/app/services/pdf.service';
import { PdfListComponent } from './pdf-list/pdf-list.component';
import { PdfComponent } from './pdf/pdf.component';
import { SharedModule } from './shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { PdfUploadComponent } from './pdf-upload/pdf-upload.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    SearchPdfComponent,
    PdfListComponent,
    PdfComponent,
    LoginComponent,
    PdfUploadComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [AuthService, PdfService],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(private _authApi: AuthService, private _router: Router) {}
}
