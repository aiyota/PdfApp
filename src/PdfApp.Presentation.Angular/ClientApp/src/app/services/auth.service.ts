import { Injectable } from '@angular/core';
import { httpGet } from '../api/api-utils';
import { ClaimsUser, User } from '../api/api.types';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export default class AuthService {
  baseUrl: string = environment.apiUrl;

  async login(token: string): Promise<ClaimsUser> {
    const response = await httpGet<{ user: ClaimsUser }>(
      `${this.baseUrl}/User/login?token=${token}`
    );
    return response.user as ClaimsUser;
  }

  async getLoggedInUser(): Promise<User> {
    const response = await httpGet<{ user: User }>(`${this.baseUrl}/User/me`);

    return response.user as User;
  }

  async isLoggedIn(): Promise<boolean> {
    try {
      await this.getLoggedInUser();
      return true;
    } catch (error) {
      return false;
    }
  }
}
