import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SearchPdfComponent } from './search-pdf/search-pdf.component';
import { LoginComponent } from './login/login.component';
import AuthService from 'src/services/auth.service';
import { Router } from '@angular/router';
import PdfService from 'src/services/pdf.service';
import { PdfListComponent } from './pdf-list/pdf-list.component';
import { PdfComponent } from './pdf/pdf.component';

@NgModule({
  declarations: [AppComponent, SearchPdfComponent, PdfListComponent, PdfComponent],
  imports: [BrowserModule, AppRoutingModule],
  providers: [AuthService, PdfService],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(private _authApi: AuthService, private _router: Router) {}
}
