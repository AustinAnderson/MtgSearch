import { Component, EventEmitter, Input, OnDestroy } from '@angular/core';
import { INegatableRemovableComponent, INegatableRemovableInput } from '../NegatableRemovable.outletspec';
import { IAccumulableQueryFragment, VisualPredicateAccumulator } from '../VisualPredicateAccumulator.service';

export type TypeLineLevel = "Supertype" | "Type" | "Subtype";
class TrackedTypeInputBox {
  constructor(private list: TrackedTypeInputBox[]) { }
  private _value = "";
  public get value() { return this._value; }
  public set value(val: string) {
    this._value = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
  public trackingId = crypto.randomUUID();
  public removeFromList() {
    let index = this.list.indexOf(this);
    if (index == -1) return;
    this.list.splice(index, 1);
    VisualPredicateAccumulator.Instance.notifyChange();
  }
  toString() {
    return this.value;
  }
}
export class TypeLineFormInput implements INegatableRemovableInput {
  removeClickedChannel = new EventEmitter<void>();
  public typeList: TrackedTypeInputBox[];
  public constructor(public isNegative: boolean, public level: TypeLineLevel) {
    this.typeList = [];
    this.typeList.push(new TrackedTypeInputBox(this.typeList));
  }

  private _isAllNotAny: boolean = true;
  public get isAllNotAny() { return this._isAllNotAny; }
  public set isAllNotAny(val: boolean) {
    this._isAllNotAny = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }

}

@Component({
  selector: 'type-line-form-fragment',
  templateUrl: './type-line-form-fragment.component.html',
  styleUrl: './type-line-form-fragment.component.css'
})
export class TypeLineFormFragmentComponent implements 
  IAccumulableQueryFragment,
  INegatableRemovableComponent<TypeLineFormInput>,
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
    let levelName = "types";
    if (this.input.level == "Subtype") levelName = "subTypes";
    if (this.input.level == "Supertype") levelName = "superTypes";
    let allAny = "any";
    if (this.input.isAllNotAny) allAny = "all";
    let query = levelName + "." + allAny + "(";
    query += this.input.typeList
      .filter(x=>x.value.length>0)
      .map(x => `"${x}"`)
      .join(",");
    query += ')';
    if (this.input.isNegative) {
      query = `(not ${query})`;
    }
    return query;
  }
  
  private _input = new TypeLineFormInput(false, "Type");
  public get input() { return this._input; }
  @Input() public set input(val: TypeLineFormInput) {
    this._input = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
  public toggleAllOrAny() {
    this.input.isAllNotAny = !this.input.isAllNotAny;
  }
  public addAnotherType() {
    this.input.typeList.push(new TrackedTypeInputBox(this.input.typeList));
  }
}
