import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';

@Component({
  selector: 'form-drop-down',
  templateUrl: './form-drop-down.component.html',
  styleUrl: './form-drop-down.component.css'
})
export class FormDropDownComponent<T> {
  @Input() emptyMessage = "--";
  @Input() options: T[] = [];

  public get maxChars() {
    let max = 0;
    for (let i = 0; i < this.options.length; i++) {
      let length = ("" + this.options[i]).length;
      if (max < length) {
        max = length;
      }
    }
    return max;
  }
  public optionsOpen = false;
  public displayClicked() {
    this.optionsOpen = !this.optionsOpen;
  }
  private _selectedOption: T | undefined;
  @Input() public set selectedOption(val: T | undefined) {
    this._selectedOption = val;
    this.selectedOptionChange.emit(val);
    this.optionsOpen = false;
  }
  public get selectedOption() { return this._selectedOption; }
  @Output() selectedOptionChange = new EventEmitter<T>();

  private clickWasIn = false;
  @HostListener('click')
  public clickin() {
    this.clickWasIn = true;
  }
  @HostListener('document:click')
  public clickout() {
    if (!this.clickWasIn) {
      this.optionsOpen = false;
    }
    this.clickWasIn = false;
  }
}
