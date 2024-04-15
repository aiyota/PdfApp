import { Component } from '@angular/core';
import { Router } from '@angular/router';
import AuthService from 'src/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [],
})
export class LoginComponent {
  private _email: string = '';
  private _password: string = '';

  constructor(private _authService: AuthService, private _router: Router) {}

  emailChange($event: Event): void {
    this._email = ($event.target as HTMLInputElement).value;
  }

  passwordChange($event: Event): void {
    this._password = ($event.target as HTMLInputElement).value;
  }

  async ngOnInit() {
    const isLoggedIn = await this._authService.isLoggedIn();
    console.log(isLoggedIn);
  }

  async login($event: SubmitEvent): Promise<void> {
    $event.preventDefault();

    try {
      const user = await this._authService.login(this._email, this._password);
      this._router.navigate(['/']);
    } catch (error) {
      alert(error);
    }
  }
}
