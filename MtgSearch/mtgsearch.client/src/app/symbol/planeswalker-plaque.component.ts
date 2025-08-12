import { Component, Input } from "@angular/core";

type plaqueDirection = "Up" | "Down" | "Neutral";
@Component({
  selector: 'planeswalker-plaque',
  templateUrl: './planeswalker-plaque.component.html'
})
export class PlaneswalkerPlaqueComponent {
  @Input() public set text(value: string | undefined) {
    if (!value) return;
    if (value.startsWith('[')) {
      value = value.replace('[', '').replace(']', '');
    }
    let symbol = value.replace(/[0-9X]+$/, '');
    let sanitizedSymbol = "\u2013";//en dash
    if (symbol == '+') {
      this.plaqueDir = "Up";
      sanitizedSymbol = '+';
    }
    else if (symbol.length == 0)
    {
      sanitizedSymbol = '';
      this.plaqueDir = "Neutral";
    }
    let sanitized = value.replace(symbol, '');
    this.value = sanitizedSymbol + sanitized;
  }
  public value: string = "+0";
  public plaqueDir:plaqueDirection = "Down";
}
