import { Component, EventEmitter, Input, OnDestroy, Output } from '@angular/core';
import { IAccumulableQueryFragment, VisualPredicateAccumulator } from '../VisualPredicateAccumulator.service';

const lessOrEq = '\u2264';
const gtOrEq = '\u2265';
export class CardAttributeType {
  private constructor(private _display: string, private _qlVal: string) { }
  public toString() { return this._display; }
  public get qlVal() { return this._qlVal; }
  public static pow = new CardAttributeType("Power", "pow");
  public static def = new CardAttributeType("Toughness", "def");
  public static mv = new CardAttributeType("Mana Value", "cmc");
  public static loyalty = new CardAttributeType("Loyalty", "loyalty");
}
class Operator {
  private static values: Operator[] = [];
  public static getValues() { return Operator.values; }
  private constructor(private _display: string, private _qlVal: string) {
    Operator.values.push(this);
  }
  public toString() { return this._display; }
  public get qlVal() { return this._qlVal; }
  public static eq = new Operator('=', '==');
  public static lt = new Operator('<', '<');
  public static lte = new Operator(lessOrEq, '<=');
  public static gt = new Operator('>', '>');
  public static gte = new Operator(gtOrEq, '>=');
  public static is = new Operator('is', 'is');
  public static has = new Operator('has', 'has');
}
@Component({
  selector: 'attribute-filter-form-fragment',
  templateUrl: './attribute-filter-form-fragment.component.html',
  styleUrl: './attribute-filter-form-fragment.component.css'
})
export class AttributeFilterFormFragmentComponent implements
  IAccumulableQueryFragment,
  OnDestroy
{
  constructor() {
    this.uuid = crypto.randomUUID();
    VisualPredicateAccumulator.Instance.register(this);
  }
  ngOnDestroy(): void {
    VisualPredicateAccumulator.Instance.remove(this.uuid);
  }
  uuid: string;
  fetchFragment(): string {
    let result = "";
    if (this.showLeftSide) {
      let op = Operator.gt;
      if (this.selectedLeftSideOp == Operator.lte) {
        op = Operator.gte;
      }
      result += `${this.attributeType.qlVal} ${op.qlVal} ${this.leftSideValue}`;
      result += " and ";
    }
    result += this.attributeType.qlVal;
    result += " ";
    result += this.selectedOperator.qlVal;
    result += " ";
    if (this.showDropDownForValues) {
      result += this.selectedIsHasValue;
    }
    else {
      result += this.rightSideValue;
    }
    return result;
  }
  @Output() public removeRequested = new EventEmitter<void>()
  @Input() public attributeType: CardAttributeType = CardAttributeType.mv;
  public operatorOptions = Operator.getValues();
  private _selectedOperator = this.operatorOptions[0];
  public get selectedOperator() { return this._selectedOperator; }
  public set selectedOperator(val: Operator) {
    this._selectedOperator = val;
    this.showDropDownForValues = val == Operator.is || val == Operator.has;
    this.showLeftSide = val == Operator.lt || val == Operator.lte;
    VisualPredicateAccumulator.Instance.notifyChange();
  }

  public showDropDownForValues = false;
  public showLeftSide = false;

  public isHasValueOptions = [
    '*',
    'X',
    'XX',
    'XXX'
  ]
  private _selectedIsHasValue = this.isHasValueOptions[0];
  public get selectedIsHasValue() { return this._selectedIsHasValue; }
  public set selectedIsHasValue(val: string) {
    this._selectedIsHasValue = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }

  public leftSideOptions = [
    Operator.lte,
    Operator.lt
  ]
  private _selectedLeftSideOp = this.leftSideOptions[0];
  public get selectedLeftSideOp() { return this._selectedLeftSideOp; }
  public set selectedLeftSideOp(val: Operator) {
    this._selectedLeftSideOp = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
  private _rightSideValue = 3;
  public get rightSideValue() { return this._rightSideValue; }
  public set rightSideValue(val: number) {
    this._rightSideValue = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }

  private _leftSideValue = 0;
  public get leftSideValue() { return this._leftSideValue; }
  public set leftSideValue(val: number) {
    this._leftSideValue = val;
    VisualPredicateAccumulator.Instance.notifyChange();
  }
}

  //power [=]
