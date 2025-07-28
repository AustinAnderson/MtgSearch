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
    this.name = value.name ?? "(Server sent empty name)";
    if (value.manaCost) {
      this.manaSymbolTexts = value.manaCost.split('{').filter(x => x.length > 0).map(x => '{' + x);
    }
    let typeLine = value.superTypes.join(' ');
    if (typeLine.length > 0) {
      typeLine += ' ';
    }
    typeLine += value.types.join(' ');
    let subTypes = value.subTypes.join(' ');
    if (subTypes.length > 0) {
      typeLine += ' — ';//it's an emdash I swear
      typeLine += subTypes;
    }
    this.typeLine = typeLine;
    if (value.power) {
      this.powerToughness = value.power + '/' + value.toughness;
    }
    else if(value.loyalty) {
      this.loyalty = value.loyalty;
    }
    if (value.textLines.length > 0) {
      for (let serverLine of value.textLines) {
        let line = { segments: [] } as BindingCardTextLine;
        for (let serverSeg of serverLine.segments) {
          let segment = new BindingCardTextLineSegment();
          segment.isHighlighted = serverSeg.isHighlighted;
          segment.isSymbol = serverSeg.isSymbol;
          segment.text = serverSeg.text;
          line.segments.push(segment);
        }
        this.textLines.push(line);
      }
    }
  }
}
