import { Component, EventEmitter, Input, OnDestroy } from '@angular/core';
import { INegatableRemovableComponent, INegatableRemovableInput } from '../NegatableRemovable.outletspec';
import { IAccumulableQueryFragment, VisualPredicateAccumulator } from '../VisualPredicateAccumulator.service';
export class NameLikeInput implements INegatableRemovableInput {
  removeClickedChannel = new EventEmitter<void>();
  public constructor(public isNegative: boolean) { }
  private _nameFragment = "";
  public get nameFragment() { return this._nameFragment; }
  public set nameFragment(val: string) {
    this._nameFragment = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
}

@Component({
  selector: 'name-like-form-fragment',
  templateUrl: './name-like-form-fragment.component.html',
  styleUrl: './name-like-form-fragment.component.css'
})
export class NameLikeFormFragmentComponent implements
  IAccumulableQueryFragment,
  INegatableRemovableComponent<NameLikeInput>,
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

  fetchFragment(): string {
    let query = `nameLike("${this.input.nameFragment}")`;
    if (this.input.isNegative) {
      query = `(not ${query})`;
    }
    return query;
  }
  private _input = new NameLikeInput(false);
  public get input() { return this._input; }
  @Input() public set input(val: NameLikeInput) {
    this._input = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
}
