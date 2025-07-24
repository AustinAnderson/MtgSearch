export class BindingCardTextLine {
  public segments: BindingCardTextLineSegment[] = [];
}
export class BindingCardTextLineSegment {
  public isSymbol: boolean = false;
  public isHighlighted: boolean = false;
  public text: string | undefined;
}
