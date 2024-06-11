import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CountriesService {

  constructor(private http : HttpClient) { }

  getCountries(): Observable<string[]> {
    return this.http.get<string[]>(environment.countriesApi+'/countries/capital');
  }
}
