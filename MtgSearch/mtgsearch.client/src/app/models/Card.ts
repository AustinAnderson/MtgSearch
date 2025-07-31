export class Card {
  public colorId: string[] = [];
  public name: undefined | string;
  public power: undefined | string;
  public toughness: undefined | string;
  public loyalty: undefined | string;
  public manaCost: undefined | string;
  public superTypes: string[] = [];
  public types: string[] = [];
  public isPreRelease: boolean = false;
  public imageUrl: undefined | string;
  public altImageUrl: undefined | string;
  public setCode: undefined | string;
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
