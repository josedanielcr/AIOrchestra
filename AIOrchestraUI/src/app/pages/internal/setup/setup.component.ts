import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputComponent } from '../../../components/input/input.component';
import { Type } from '../../../components/input/input.enum';
import { ApiGatewayUserManagementService } from '../../../services/api-gateway-user-management.service';
import { interval, takeWhile } from 'rxjs';
import { CountriesService } from '../../../services/countries.service';

@Component({
  selector: 'app-setup',
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent],
  templateUrl: './setup.component.html',
  styleUrl: './setup.component.css'
})
export class SetupComponent implements OnInit {

  form: FormGroup;
  InputType = Type;

  constructor(private fb: FormBuilder, 
    public userManagementService : ApiGatewayUserManagementService,
    private countriesService: CountriesService){

    this.form = this.fb.group({
      name: ['', Validators.required],
      email: [{ value: '' , disabled: true }, [Validators.required, Validators.email]],
      nickname: ['', Validators.required],
      age: ['', [Validators.required, Validators.min(1)]],
      country: ['', Validators.required],
      genre: ['', Validators.required],
      language: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.countriesService.getCountries().subscribe((countries : any) => {
      console.log(countries['data']);
    });
  }

  onSubmit() {
    console.log(this.form.value);
  }
}
