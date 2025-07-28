import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
  selector: 'color-id-selector',
  templateUrl: './color-id-selector.component.html'
})
export class ColorIdSelectorComponent {
  private _whiteSelected: boolean = false;
  private _blueSelected: boolean = false;
  private _blackSelected: boolean = false;
  private _redSelected: boolean = false;
  private _greenSelected: boolean = false;
  private _colorlessSelected: boolean = true;

  @Output() onValueChanged = new EventEmitter<string>();

  public get WhiteSelected(): boolean { return this._whiteSelected; }
  @Input() public set WhiteSelected(value: boolean) {
    this.ColorlessSelected = false;
    this._whiteSelected = value;
    this.SetColorlessTrueIfAllUnchecked();
    this.NotifyUpdateColorString();
  }
  public ToggleWhite() { this.WhiteSelected = !this._whiteSelected; }



  public get BlueSelected(): boolean { return this._blueSelected; }
  @Input() public set BlueSelected(value: boolean) {
    this.ColorlessSelected = false;
    this._blueSelected = value;
    this.SetColorlessTrueIfAllUnchecked();
    this.NotifyUpdateColorString();
  }
  public ToggleBlue() { this.BlueSelected = !this._blueSelected; }


  public get BlackSelected(): boolean { return this._blackSelected; }
  @Input() public set BlackSelected(value: boolean) {
    this.ColorlessSelected = false;
    this._blackSelected = value;
    this.SetColorlessTrueIfAllUnchecked();
    this.NotifyUpdateColorString();
  }
  public ToggleBlack() { this.BlackSelected = !this._blackSelected; }


  public get RedSelected(): boolean { return this._redSelected; }
  @Input() public set RedSelected(value: boolean) {
    this.ColorlessSelected = false;
    this._redSelected = value;
    this.SetColorlessTrueIfAllUnchecked();
    this.NotifyUpdateColorString();
  }
  public ToggleRed() { this.RedSelected = !this._redSelected; }


  
  public get GreenSelected(): boolean { return this._greenSelected; }
  @Input() public set GreenSelected(value: boolean) {
    this.ColorlessSelected = false;
    this._greenSelected = value;
    this.SetColorlessTrueIfAllUnchecked();
    this.NotifyUpdateColorString();
  }
  public ToggleGreen() { this.GreenSelected = !this._greenSelected; }



  public get ColorlessSelected(): boolean { return this._colorlessSelected; }
  @Input() public set ColorlessSelected(value: boolean) {
    if (value) {
      if (this._whiteSelected) this.WhiteSelected = false;
      if (this._blueSelected) this.BlueSelected = false;
      if (this._blackSelected) this.BlackSelected = false;
      if (this._redSelected) this.RedSelected = false;
      if (this._greenSelected) this.GreenSelected = false;
    }
    this._colorlessSelected = value;
    this.NotifyUpdateColorString();
  }
  public ToggleColorless() {
    this.ColorlessSelected = !this._colorlessSelected;
    if (!this._whiteSelected && !this._blueSelected && !this._blackSelected && !this._redSelected && !this._greenSelected && !this._colorlessSelected) {
      this.WhiteSelected = true;
      this.BlueSelected = true;
      this.BlackSelected = true;
      this.RedSelected = true;
      this.GreenSelected = true;
    }
  }
  private SetColorlessTrueIfAllUnchecked() {
    if (!this._whiteSelected && !this._blueSelected && !this._blackSelected && !this._redSelected && !this._greenSelected) {
      this.ColorlessSelected = true;
    }
  }
  private NotifyUpdateColorString() {
    let colorId = "";
    if (this.WhiteSelected) colorId += "W";
    if (this.BlueSelected) colorId += "U";
    if (this.BlackSelected) colorId += "B";
    if (this.RedSelected) colorId += "R";
    if (this.GreenSelected) colorId += "G";
    this.onValueChanged.emit(colorId);
  }
}
