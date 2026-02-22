import { Component, EventEmitter, Input, OnDestroy } from '@angular/core';
import { IAccumulableQueryFragment, VisualPredicateAccumulator } from '../VisualPredicateAccumulator.service';
import { INegatableRemovableComponent, INegatableRemovableInput } from '../NegatableRemovable.outletspec';

export class CharSepInputs implements INegatableRemovableInput {
  constructor(public isNegative: boolean, public separatorChar: string) {}
  public removeClickedChannel = new EventEmitter<void>();
  private _preSep: string = "";
  public get preSep() { return this._preSep; }
  public set preSep(val: string) {
    this._preSep = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
  private _postSep: string = "";
  public get postSep() { return this._postSep; }
  public set postSep(val: string) {
    this._postSep = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
}

@Component({
  selector: 'char-separated-form-fragment',
  templateUrl: './char-separated-form-fragment.component.html',
  styleUrl: './char-separated-form-fragment.component.css'
})
export class CharSeparatedFormFragmentComponent implements
  IAccumulableQueryFragment,
  INegatableRemovableComponent<CharSepInputs>,
  OnDestroy 
{
  constructor() {
    this.uuid = crypto.randomUUID();
    VisualPredicateAccumulator.Instance.register(this);
  }
  ngOnDestroy(): void {
    VisualPredicateAccumulator.Instance.remove(this.uuid);
  }
  public uuid: string;
  public fetchFragment(): string {
    let regex = VisualPredicateAccumulator.escapeRegex(this.input.preSep + "...")
      .replaceAll('\\.\\.\\.', '[^' + this.input.separatorChar + ']*')
      + this.input.separatorChar
      + VisualPredicateAccumulator.escapeRegex("..." + this.input.postSep).replaceAll('\\.\\.\\.', '.*');
    regex = regex.replaceAll("{\\?}", "{.}");
    let query = `text("${regex}")`;
    if (this.input.isNegative) {
      query = `(not ${query})`;
    }
    return query;
  }
  private _input = new CharSepInputs(true,',');
  public get input() { return this._input; }
  @Input() public set input(val: CharSepInputs) {
    this._input = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
}
