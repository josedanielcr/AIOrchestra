import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputComponent } from '../../../components/input/input.component';
import { Type } from '../../../components/input/input.enum';
import { ApiGatewayUserManagementService } from '../../../services/api-gateway-user-management.service';
import { CountriesService } from '../../../services/countries.service';
import { SearchSelectComponent } from '../../../components/search-select/search-select.component';
import { Option } from '../../../components/search-select/option.model';
import { LanguageService } from '../../../services/language.service';
import { GenreService } from '../../../services/genre.service';
import { ButtonComponent } from '../../../components/button/button.component';
import { ButtonType } from '../../../components/button/type.enum';
import { EthnicityService } from '../../../services/ethnicity.service';

@Component({
  selector: 'app-setup',
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent, SearchSelectComponent, ButtonComponent],
  templateUrl: './setup.component.html',
  styleUrl: './setup.component.css'
})
export class SetupComponent implements OnInit {

  public form: FormGroup = new FormGroup({});
  public InputType = Type;
  public buttonType = ButtonType;
  public countryOptions : Option[] = [];
  public languageOptions = this.languageService.getLanguages();
  public genreOptions = this.genreService.getGenres();
  public ethnicityOptions = this.ethnicityService.getEthnicities();

  constructor(private fb: FormBuilder, 
    public userManagementService : ApiGatewayUserManagementService,
    private countriesService: CountriesService,
    private languageService : LanguageService,
    private genreService : GenreService,
    private ethnicityService : EthnicityService) {
      this.onSubmit = this.onSubmit.bind(this);
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      nickname: ['', Validators.required],
      age: ['', [Validators.required, Validators.min(1)]],
      country: ['', Validators.required],
      genre: ['', Validators.required],
      language: ['', Validators.required],
      ethnicity : ['', Validators.required],
    });
    this.setCountries();
  }

  private setCountries() {
    this.countriesService.getCountries().subscribe((countries: any) => {
      this.countryOptions = countries.data.map((country: any) => {
        return { label: country.name, value: country.iso3 };
      });
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.userManagementService.setupUser(this.form.value).subscribe((response: any) => {
        this.userManagementService.setUser(response.data);
        console.log(response);
      });
    }
  }
}