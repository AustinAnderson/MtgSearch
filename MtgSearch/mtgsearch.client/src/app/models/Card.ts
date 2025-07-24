export class Card {
  public Name: undefined | string;
  public Power: undefined | string;
  public Toughness: undefined | string;
  public Loyalty: undefined | string;
  public ManaCost: undefined | string;
  public SuperTypes: string[] = [];
  public Types: string[] = [];
  public SubTypes: string[] = [];
  public TextLines: CardTextLine[] = [];
}
export class CardTextLine {
  public Segments: CardTextLineSegment[] = [];
}
export class CardTextLineSegment {
  public IsSymbol: boolean = false;
  public IsHighlighted: boolean = false;
  public Text: undefined | string;
}
