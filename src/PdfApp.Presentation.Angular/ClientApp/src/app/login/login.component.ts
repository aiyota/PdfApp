import { Component } from '@angular/core';
import { Router } from '@angular/router';
import AuthService from 'src/app/services/auth.service';
import { LoadingSpinnerComponent } from '../shared/loading-spinner/loading-spinner.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [],
})
export class LoginComponent {
  private _email: string = '';
  private _password: string = '';
  public isLoading: boolean = false;

  constructor(private _authService: AuthService, private _router: Router) {}

  emailChange($event: Event): void {
    this._email = ($event.target as HTMLInputElement).value;
  }

  passwordChange($event: Event): void {
    this._password = ($event.target as HTMLInputElement).value;
  }

  async login($event: SubmitEvent): Promise<void> {
    $event.preventDefault();

    try {
      this.isLoading = true;
      const user = await this._authService.login(this._email, this._password);
      this.isLoading = false;

      if (user) {
        this._router.navigate(['/']);
        return;
      }
      // Handle error
    } catch (error) {
      alert(error);
      this.isLoading = false;
    }
  }
}
