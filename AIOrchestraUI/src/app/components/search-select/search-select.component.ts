import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, forwardRef } from '@angular/core';
import { AbstractControl, ControlValueAccessor, FormsModule, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';
import { Option } from './option.model';

@Component({
  selector: 'app-search-select',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './search-select.component.html',
  styleUrl: './search-select.component.css',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SearchSelectComponent),
      multi: true
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => SearchSelectComponent),
      multi: true
    }
  ]
})
export class SearchSelectComponent implements OnInit, ControlValueAccessor, Validator {

  @Input() value: any = null;
  @Input() disabled: boolean = false; 
  @Input() label: string = '';
  @Input() options: Option[] = [];

  filteredOptions: Option[] = [];
  searchValue: string = '';
  isOpen: boolean = false;

  private onChange = (value: any) => {};
  private onTouched = () => {};

  ngOnInit() {
    this.filteredOptions = this.options;
  }

  writeValue(value: any): void {
    this.value = value;
    this.updateFilteredOptions();
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  validate(control: AbstractControl): ValidationErrors | null {
    return null;
  }

  onOptionSelect(option: any) {
    this.value = option.value;
    this.onChange(this.value);
    this.onTouched();
    this.isOpen = false;
  }

  onSearchChange(search: string) {
    this.searchValue = search.toLowerCase();
    this.updateFilteredOptions();
  }


  toggleDropdown() {
    if (!this.disabled) {
      this.isOpen = !this.isOpen;
    }
  }

  closeDropdown() {
    this.isOpen = false;
  }

  getDisplayValue(): string {
    const selectedOption = this.options.find(option => option.value === this.value);
    return selectedOption ? selectedOption.label : '';
  }

  private updateFilteredOptions() {
    this.filteredOptions = this.options.filter(option =>
      option.label.toLowerCase().includes(this.searchValue)
    );
  }
}