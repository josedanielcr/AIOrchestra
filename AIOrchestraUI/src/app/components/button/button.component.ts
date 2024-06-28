import { Component, Input } from '@angular/core';
import { ButtonType } from './type.enum';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './button.component.html',
  styleUrl: './button.component.css'
})
export class ButtonComponent {

  @Input() label: string = '';
  @Input() event: Function = () => {};
  @Input() type: ButtonType = ButtonType.PRIMARY;
  @Input() icon : string = '';

  constructor() { }

  public handleClick() {
    this.event();
  }

  getButtonClass(): string {
    switch (this.type) {
      case ButtonType.PRIMARY:
        return 'btn-primary';
      case ButtonType.SECONDARY:
        return 'btn-secondary';
      case ButtonType.TERTIARY:
        return 'btn-tertiary';
      case ButtonType.DANGER:
        return 'btn-danger';
      default:
        return '';
    }
  }
}