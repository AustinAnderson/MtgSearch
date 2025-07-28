export class Card {
  public name: undefined | string;
  public power: undefined | string;
  public toughness: undefined | string;
  public loyalty: undefined | string;
  public manaCost: undefined | string;
  public superTypes: string[] = [];
  public types: string[] = [];
  public subTypes: string[] = [];
  public textLines: CardTextLine[] = [];
}
export class CardTextLine {
  public segments: CardTextLineSegment[] = [];
}
export class CardTextLineSegment {
  public isSymbol: boolean = false;
  public isHighlighted: boolean = false;
  public text: undefined | string;
}
