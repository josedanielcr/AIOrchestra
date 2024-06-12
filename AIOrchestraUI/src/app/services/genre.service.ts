import { Injectable } from '@angular/core';
import { Option } from '../components/search-select/option.model';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  constructor() { }

  getGenres(): Option[] {
    const languageList = [
      'None',
      'Male',
      'Female',
      'Other'
    ];

    return languageList.map((language: string) => {
      return { label: language, value: language };
    });
  }
}