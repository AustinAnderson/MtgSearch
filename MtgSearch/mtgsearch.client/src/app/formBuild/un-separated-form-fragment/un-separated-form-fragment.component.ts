import { Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { IAccumulableQueryFragment, VisualPredicateAccumulator } from '../VisualPredicateAccumulator.service';
import { INegatableRemovableComponent, INegatableRemovableInput } from '../NegatableRemovable.outletspec';

export class UnSepInputs implements INegatableRemovableInput{
  constructor(public isNegative: boolean) {}
  [key: string]: unknown;
  public removeClickedChannel = new EventEmitter<void>();

  private _queryText: string = "";
  public get queryText() { return this._queryText; }
  public set queryText(val: string) {
    this._queryText = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
}
@Component({
  selector: 'un-separated-form-fragment',
  templateUrl: './un-separated-form-fragment.component.html',
  styleUrl: './un-separated-form-fragment.component.css'
})
export class UnSeparatedFormFragmentComponent implements
  IAccumulableQueryFragment,
  INegatableRemovableComponent<UnSepInputs>,
  OnDestroy {
  constructor() {
    this.uuid = crypto.randomUUID();
    VisualPredicateAccumulator.Instance.register(this);
  }
  ngOnDestroy(): void {
    VisualPredicateAccumulator.Instance.remove(this.uuid);
  }
  public uuid: string;
  public fetchFragment(): string {
    let regex = VisualPredicateAccumulator.escapeRegex(this.input.queryText).replaceAll('\\.\\.\\.', '.*');
    regex = regex.replaceAll("{\\?}", "{.}");
    let query = `text("${regex}")`;
    if (this.input.isNegative) {
      query = `(not ${query})`;
    }
    return query;
  }

  private _input = new UnSepInputs(true);
  public get input() { return this._input; }
  @Input() public set input(val: UnSepInputs) {
    this._input = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
}
