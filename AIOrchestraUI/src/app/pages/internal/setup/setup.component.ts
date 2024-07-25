import { AfterViewInit, Component, EventEmitter, inject, Input, input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { InputComponent } from '../../../components/input/input.component';
import { Type } from '../../../components/input/input.enum';
import { ApiGatewayUserManagementService } from '../../../services/api-gateway-user-management.service';
import { SearchSelectComponent } from '../../../components/search-select/search-select.component';
import { Option } from '../../../components/search-select/option.model';
import { ButtonComponent } from '../../../components/button/button.component';
import { ButtonType } from '../../../components/button/type.enum';
import { Router, RouterModule } from '@angular/router';
import { SliderComponent } from '../../../components/slider/slider.component';
import { AppUser } from '../../../models/user';
import { Subscription } from 'rxjs';
import { NgToastService } from 'ng-angular-popup';
@Component({
  selector: 'app-setup',
  standalone: true,
  imports: [ReactiveFormsModule, InputComponent, SearchSelectComponent, ButtonComponent, RouterModule, SliderComponent],
  templateUrl: './setup.component.html',
  styleUrl: './setup.component.css'
})
export class SetupComponent implements OnInit, OnDestroy {

  public form: FormGroup = new FormGroup({});
  public InputType = Type;
  public buttonType = ButtonType;
  @Input() isNewUser : boolean = true;
  @Output() onUpdate : EventEmitter<boolean> = new EventEmitter<boolean>();
  private subscriptions : Subscription[] = [];
  toast = inject(NgToastService);

  constructor(private fb: FormBuilder, 
    public userManagementService : ApiGatewayUserManagementService,
    private router: Router) {
      this.onSubmit = this.onSubmit.bind(this);
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      nickname: ['', Validators.required],
      age: ['', [Validators.required, Validators.min(1)]],
      danceability: [this.userManagementService.user()?.Danceability == null ? 50 : this.userManagementService.user()?.Danceability, [Validators.required, Validators.min(0), Validators.max(100)]],
      energy: [this.userManagementService.user()?.Energy == null ? 50 : this.userManagementService.user()?.Energy, [Validators.required, Validators.min(0), Validators.max(100)]],
      speechiness: [this.userManagementService.user()?.Speechiness == null ? 50 : this.userManagementService.user()?.Speechiness , [Validators.required, Validators.min(0), Validators.max(100)]],
      loudness: [this.userManagementService.user()?.Loudness == null ? 50 : this.userManagementService.user()?.Loudness, [Validators.required, Validators.min(0), Validators.max(100)]],
      instrumentalness: [this.userManagementService.user()?.Instrumentalness == null ? 50 : this.userManagementService.user()?.Instrumentalness, [Validators.required, Validators.min(0), Validators.max(100)]],
      liveness: [this.userManagementService.user()?.Liveness == null ? 50 : this.userManagementService.user()?.Liveness, [Validators.required, Validators.min(0), Validators.max(100)]]
    });
  }

  onSubmit() {
    if(this.isNewUser){
      if(!this.form.valid) return;
    } else {
      this.updateUserPreferences();
    }
    this.subscriptions.push(
      this.userManagementService.setupUser(this.form.value).subscribe({
        next : (response : any) => {
          this.userManagementService.setUser(response.value);
          if(!this.isNewUser) this.onUpdate.emit(true);
          else this.router.navigate(['/home/dashboard']);
        },
        error : () => {
          this.toast.danger('An error occurred while trying to setup your account');
        }
      })
    );
  }

  private updateUserPreferences() {
    const user = this.userManagementService.user();
    this.form.get('name')?.setValue(user?.Name);
    this.form.get('email')?.setValue(user?.Email);
    this.form.get('nickname')?.setValue(user?.Nickname);
    this.form.get('age')?.setValue(user?.Age);
  }
}