import { Component, Input } from "@angular/core";
import { Card } from "../models/Card";
import { BindingCardTextLine, BindingCardTextLineSegment } from "./card.bindingmodel";

@Component({
  selector: 'card',
  templateUrl: './card.component.html'
})
export class CardComponent {
  public name: string = "loading error";
  public altFaceName: string | undefined;
  public manaSymbolTexts: string[] = [];
  public typeLine: string = "loading -- error";
  public powerToughness: string|undefined;
  public loyalty: string | undefined;
  public textLines: BindingCardTextLine[] = [];
  @Input() public set data(value: Card)
  {
    this.name = value.Name ?? "(Server sent empty name)";
    if (value.ManaCost) {
      this.manaSymbolTexts = value.ManaCost.split('{').filter(x => x.length > 0).map(x => '{' + x);
    }
    let typeLine = value.SuperTypes.join(' ');
    if (typeLine.length > 0) {
      typeLine += ' ';
    }
    typeLine += value.Types.join(' ');
    let subTypes = value.SubTypes.join(' ');
    if (subTypes.length > 0) {
      typeLine += ' — ';//it's an emdash I swear
      typeLine += subTypes;
    }
    this.typeLine = typeLine;
    if (value.Power) {
      this.powerToughness = value.Power + '/' + value.Toughness;
    }
    else if(value.Loyalty) {
      this.loyalty = value.Loyalty;
    }
    if (value.TextLines.length > 0) {
      for (let serverLine of value.TextLines) {
        let line = { segments: [] } as BindingCardTextLine;
        for (let serverSeg of serverLine.Segments) {
          let segment = new BindingCardTextLineSegment();
          segment.isHighlighted = serverSeg.IsHighlighted;
          segment.isSymbol = serverSeg.IsSymbol;
          segment.text = serverSeg.Text;
          line.segments.push(segment);
        }
        this.textLines.push(line);
      }
    }
  }
}
