import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AppComponent } from './app.component';
import { SearchPdfComponent } from './search-pdf/search-pdf.component';
import { AuthGuard } from 'src/guards/auth.guard';
import { PdfComponent } from './pdf/pdf.component';

const routes: Routes = [
  {
    path: '',
    component: SearchPdfComponent,

    canActivate: [AuthGuard],
  },
  { path: 'login', component: LoginComponent },
  { path: 'pdf/:id', component: PdfComponent, canActivate: [AuthGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
