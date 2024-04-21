import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import AuthService from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  async canActivate(): Promise<boolean | UrlTree> {
    const isLoggedIn = await this.authService.isLoggedIn();

    if (!isLoggedIn) {
      return this.router.parseUrl('/login');
    }

    return true;
  }
}
