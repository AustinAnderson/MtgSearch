import { Component, Input } from "@angular/core";
@Component({
  selector: 'card-symbol',
  templateUrl: './card-symbol.component.html',
  styleUrl: './scryfallSymbols.css'
})
export class CardSymbolComponent
{
  public classList: string[] = [];
  public symbolTitle: string = "not loaded";
  public text: string = "{?}";
  @Input() public set symbolText(value: string | undefined) {
    let valueSanitized = value ?? "{?}";
    this.text = valueSanitized;
    this.classList.push('card-symbol');
    if (/{[0-9][0-9]?}/.test(valueSanitized)) {
      let num = parseInt(valueSanitized.replace('{', '').replace('}', ''));
      this.classList.push('card-symbol-' + num);
      this.symbolTitle = '' + num + ' generic mana';
    }
    else {
      let classNote = valueSanitized.replace('{', '').replace('}', '').replace('/', '');
      this.classList.push('card-symbol-' + classNote);
      this.symbolTitle = CardSymbolComponent.SymbolToolTipMap[valueSanitized];
      if (this.symbolTitle === 'undefined') {
        this.symbolTitle = 'symbol is missing a css icon';
      }
    }
  }
  //keys should be the set of every symbol currently used in magic, except funny (crank/sticker) which should be filtered by the api
  private static readonly SymbolToolTipMap: {[index:string]:string} = {
    '{Q}': 'the untap symbol',
    '{T}': 'the tap symbol',
    '{W}': 'one white mana',
    '{U}': 'one blue mana',
    '{B}': 'one black mana',
    '{R}': 'one red mana',
    '{G}': 'one green mana',
    '{C}': 'one colorless mana',
    '{S}': 'one mana from any snow source',
    '{P}': 'modal budget pawprint',
    '{E}': 'energy counter',
    '{X}': 'choose a value of generic mana to pay',
    '{W/P}': 'one white mana or two life',
    '{U/P}': 'one blue mana or two life',
    '{B/P}': 'one black mana or two life',
    '{R/P}': 'one red mana or two life',
    '{G/P}': 'one green mana or two life',
    '{C/W}': 'one white or one colorless mana',
    '{C/U}': 'one blue or one colorless mana',
    '{C/B}': 'one black or one colorless mana',
    '{C/R}': 'one red or one colorless mana',
    '{C/G}': 'one green or one colorless mana',
    '{2/W}': 'one white or two generic mana',
    '{2/U}': 'one blue or two generic mana',
    '{2/B}': 'one black or two generic mana',
    '{2/R}': 'one red or two generic mana',
    '{2/G}': 'one green or two generic mana',
    '{R/W}': 'one red or one white mana',
    '{W/B}': 'one white or one black mana',
    '{B/G}': 'one black or one green mana',
    '{U/B}': 'one blue or one black mana',
    '{G/W}': 'one green or one white mana',
    '{B/R}': 'one black or one red mana',
    '{W/U}': 'one white or one blue mana',
    '{G/U}': 'one green or one blue mana',
    '{U/R}': 'one blue or one red mana',
    '{R/G}': 'one red or one green mana'
  }
}
