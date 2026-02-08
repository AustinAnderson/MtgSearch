import { Component, Input, OnDestroy } from '@angular/core';
import { IAccumulableQueryFragment, VisualPredicateAccumulator } from '../VisualPredicateAccumulator.service';

@Component({
  selector: 'app-comma-separated',
  templateUrl: './comma-separated.component.html',
  styleUrl: './comma-separated.component.css'
})
export class CommaSeparatedComponent implements IAccumulableQueryFragment, OnDestroy {
  constructor() {
    this.uuid = crypto.randomUUID();
    VisualPredicateAccumulator.Instance.register(this);
  }
  ngOnDestroy(): void {
    VisualPredicateAccumulator.Instance.remove(this.uuid);
  }
  public uuid: string;
  public fetchFragment(): string {
    let regex = VisualPredicateAccumulator.escapeRegex(this.preSep)
      .replace('\\.\\.\\.', '[^' + this.separatorChar + ']*')
      + this.separatorChar;
      + VisualPredicateAccumulator.escapeRegex(this.postSep).replace('\\.\\.\\.', '.*');
    let query = `text("${regex}")`;
    if (this.isNegative) {
      query = `(not ${query})`;
    }
    return query;
  }
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
  private _separatorChar: string = ',';
  public get separatorChar() { return this._separatorChar; }
  @Input() public set separatorChar(val: string) {
    this._separatorChar = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
  private _isNegative = false;
  public get isNegative() { return this._isNegative; }
  @Input() public set isNegative(val: boolean) {
    this._isNegative = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
}
