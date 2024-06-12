import { Injectable } from '@angular/core';
import { Option } from '../components/search-select/option.model';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  constructor() { }

  getLanguages(): Option[] {
    const languagesList = [
      'English',
      'Spanish',
      'French',
      'German',
      'Italian',
      'Portuguese',
      'Dutch',
      'Russian',
      'Chinese',
      'Japanese',
      'Korean',
      'Arabic',
      'Hindi',
      'Bengali',
      'Punjabi',
      'Urdu',
      'Vietnamese',
      'Turkish',
      'Persian',
      'Thai',
      'Polish',
      'Ukrainian',
      'Romanian',
      'Greek',
      'Swedish',
      'Norwegian',
      'Danish',
      'Finnish',
      'Hungarian',
      'Czech',
      'Slovak',
      'Bulgarian',
      'Croatian',
      'Serbian',
      'Slovenian',
      'Macedonian',
      'Albanian',
      'Lithuanian',
      'Latvian',
      'Estonian',
      'Maltese',
      'Icelandic',
      'Irish',
      'Welsh',
      'Basque',
      'Catalan',
      'Galician',
      'ScottishGaelic',
      'Breton',
      'Corsican',
      'Luxembourgish',
      'Faroese',
      'Greenlandic',
      'Sami',
      'Esperanto',
      'Latin',
      'Klingon',
      'None'
    ];

    return languagesList.map(language => ({ label: language, value: language }));
  }
}
