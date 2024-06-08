import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@auth0/auth0-angular';
import { Observable } from 'rxjs';
import { BaseResponse } from '../models/base.response';
import { environment } from '../../environments/environment.development';
import { AppUser } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class ApiGatewayUserManagementService {

  private user : User | null = null;

  constructor(private http : HttpClient) { }

  public setUser(userData: User) {
    this.user = userData;
  }

  public getUser(): User | null {
    return this.user;
  }

  public clearUser() {
    this.user = null;
  }

  public createUserIfNotExists(user : User): Observable<BaseResponse<AppUser>> {
    return this.http.post<BaseResponse<AppUser>>(environment.apiGateway+'/api/user', user);
  }
}
