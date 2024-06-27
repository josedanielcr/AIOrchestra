import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputComponent } from '../../../components/input/input.component';
import { Type } from '../../../components/input/input.enum';
import { ApiGatewayUserManagementService } from '../../../services/api-gateway-user-management.service';
import { SearchSelectComponent } from '../../../components/search-select/search-select.component';
import { Option } from '../../../components/search-select/option.model';
import { ButtonComponent } from '../../../components/button/button.component';
import { ButtonType } from '../../../components/button/type.enum';
import { Router, RouterModule } from '@angular/router';
@Component({
  selector: 'app-setup',
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent, SearchSelectComponent, ButtonComponent, RouterModule],
  templateUrl: './setup.component.html',
  styleUrl: './setup.component.css'
})
export class SetupComponent implements OnInit {

  public form: FormGroup = new FormGroup({});
  public InputType = Type;
  public buttonType = ButtonType;

  public options: Option[] = [
    new Option("Dislike it strongly", 0),
    new Option("Dislike it", 25),
    new Option("Neutral", 50),
    new Option("Like it", 75),
    new Option("Like it a lot", 100)
  ];

  constructor(private fb: FormBuilder, 
    public userManagementService : ApiGatewayUserManagementService,
    private router: Router) {
      this.onSubmit = this.onSubmit.bind(this);
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      nickname: ['', Validators.required],
      age: ['', [Validators.required, Validators.min(1)]],
      danceability: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
      energy: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
      speechiness: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
      loudness: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
      instrumentalness: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
      liveness: ['', [Validators.required, Validators.min(0), Validators.max(100)]]
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.userManagementService.setupUser(this.form.value).subscribe((response: any) => {
        this.userManagementService.setUser(response.data);
        this.router.navigate(['/home/dashboard']);
      });
    }
  }
}