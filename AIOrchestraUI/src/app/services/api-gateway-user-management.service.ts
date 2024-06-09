import { HttpClient } from '@angular/common/http';
import { Injectable, WritableSignal, signal } from '@angular/core';
import { User } from '@auth0/auth0-angular';
import { BehaviorSubject, Observable } from 'rxjs';
import { BaseResponse } from '../models/base.response';
import { environment } from '../../environments/environment.development';
import { AppUser } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class ApiGatewayUserManagementService {

  public user : WritableSignal<AppUser | null> = signal<AppUser | null>(null);

  constructor(private http : HttpClient) { }

  public setUser(userData: AppUser) {
    this.user.set(userData);
  }

  public clearUser() {
    this.user.set(null);
  }

  public createUserIfNotExists(user : User): Observable<BaseResponse<AppUser>> {
    return this.http.post<BaseResponse<AppUser>>(environment.apiGateway+'/api/user', user);
  }
}
