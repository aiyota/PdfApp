import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import AuthService from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [],
})
export class LoginComponent implements OnInit {
  public isLoading: boolean = false;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _authService: AuthService
  ) {}

  ngOnInit(): void {
    this._route.queryParams.subscribe((params) => {
      const token = params['token'];
      if (token) {
        this.sendTokenRequest(token);
      }
    });
  }

  private async sendTokenRequest(token: string): Promise<void> {
    try {
      this.isLoading = true;
      const user = await this._authService.login(token);
      this.isLoading = false;

      if (user) {
        this._router.navigate(['/']);
        return;
      }
    } catch (error) {
      console.error('Error during token request:', error);
      this.isLoading = false;
    }
  }
}
