import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import appRoutes from 'src/app/routing/app-routes';

export type RouteRecord = {
  name: string;
  path: string;
};

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss'],
})
export class NavbarComponent {
  routes: RouteRecord[] = [
    { name: 'Home', path: appRoutes.home },
    { name: 'Pdf', path: appRoutes.pdf },
    { name: 'Pdf Upload', path: appRoutes.pdfUpload },
  ];
}
