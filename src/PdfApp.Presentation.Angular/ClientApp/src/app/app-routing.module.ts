import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { SearchPdfComponent } from './pdf-search/pdf-search.component';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { PdfComponent } from './pdf/pdf.component';
import appRoutes from 'src/app/routing/app-routes';
import { PdfUploadComponent } from './pdf-upload/pdf-upload.component';

const routes: Routes = [
  {
    path: '',
    component: SearchPdfComponent,
    canActivate: [AuthGuard],
  },
  { path: appRoutes.login, component: LoginComponent },
  {
    path: appRoutes.pdfById,
    component: PdfComponent,
    canActivate: [AuthGuard],
  },
  {
    path: appRoutes.pdfUpload,
    component: PdfUploadComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
