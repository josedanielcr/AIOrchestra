import { Injectable } from '@angular/core';
import { Option } from '../components/search-select/option.model';

@Injectable({
  providedIn: 'root'
})
export class EthnicityService {

  constructor() { }

  getEthnicities(): Option[] {
    const ethnicityList: string[] = [
      'Asian',
      'European',
      'HispanicOrLatino',
      'MiddleEastern',
      'NativeAmerican',
      'PacificIslander',
      'MixedOrMultiple',
      'PreferNotToSay',
      'Other',
      'None'
    ];
    
    return ethnicityList.map((ethnicity: string) => {
      return { label: ethnicity, value: ethnicity };
    });
  }
}
