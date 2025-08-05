import { Component, Input } from "@angular/core";
import { Card } from "../models/Card";
import { BindingCardTextLine, BindingCardTextLineSegment } from "./card.bindingmodel";

@Component({
  selector: 'card',
  templateUrl: './card.component.html'
})
export class CardComponent {
  public cardColor: string = "colorC";
  public name: string = "loading error";
  public altFaceName: string | undefined;
  public manaSymbolTexts: string[] = [];
  public typeLine: string = "loading -- error";
  public powerToughness: string|undefined;
  public loyalty: string | undefined;
  public textLines: BindingCardTextLine[] = [];
  public preRelease: boolean = false;
  public imageUrl: string | undefined;
  public altImageUrl: string | undefined;
  public hasAlt: boolean = false;

  @Input() public set data(value: Card)
  {
    this.imageUrl = value.imageUrl;
    this.altFaceName = value.altFaceName;
    this.altImageUrl = value.altImageUrl;
    this.hasAlt = !!this.altFaceName
    this.preRelease = value.isPreRelease;
    if (value.colorId.length > 2) {
      this.cardColor = "colorMulti";
    }
    else if (value.colorId.length == 1) {
      this.cardColor = "color" + value.colorId[0].toUpperCase();
    }
    else if (value.colorId.length == 2) {
      this.cardColor = "color" + value.colorId.sort((x, y) => x.localeCompare(y)).map(x=>x.toUpperCase()).join('');
    }
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
