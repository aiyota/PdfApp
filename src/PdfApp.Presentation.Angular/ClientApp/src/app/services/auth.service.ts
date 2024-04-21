import { Inject } from '@angular/core';
import { httpGet, httpPost } from '../api/api-utils';
import { LoginRequest, User } from '../api/api.types';

@Inject({ providedIn: 'root' })
export default class AuthService {
  async login(email: string, password: string): Promise<User> {
    // hardcoded url for now
    const response = await httpPost<LoginRequest, { user?: User }>(
      'https://localhost:7017/api/User/login',
      {
        email,
        password,
      }
    );

    if (!response.user) {
      throw new Error('Invalid credentials');
    }

    return response.user as User;
  }

  async getLoggedInUser(): Promise<User> {
    const response = await httpGet<{ user: User }>(
      'https://localhost:7017/api/User/me'
    );

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
