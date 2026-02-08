import { Component, Input, OnDestroy } from '@angular/core';
import { IAccumulableQueryFragment, VisualPredicateAccumulator } from '../VisualPredicateAccumulator.service';

@Component({
  selector: 'app-un-separated-form-fragment',
  templateUrl: './un-separated-form-fragment.component.html',
  styleUrl: './un-separated-form-fragment.component.css'
})
export class UnSeparatedFormFragmentComponent implements IAccumulableQueryFragment, OnDestroy{
  constructor() {
    this.uuid = crypto.randomUUID();
    VisualPredicateAccumulator.Instance.register(this);
  }
  ngOnDestroy(): void {
    VisualPredicateAccumulator.Instance.remove(this.uuid);
  }
  public uuid: string;
  public fetchFragment(): string {
    let regex = VisualPredicateAccumulator.escapeRegex(this.queryText).replace('\\.\\.\\.', '.*');
    let query= `text("${regex}")`;
    if (this.isNegative) {
      query = `(not ${query})`;
    }
    return query;
  }
  private _queryText = "";
  public get queryText() { return this._queryText; }
  public set queryText(val: string) {
    this._queryText = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
  private _isNegative = false;
  public get isNegative() { return this._isNegative; }
  @Input() public set isNegative(val: boolean) {
    this._isNegative = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }

}
