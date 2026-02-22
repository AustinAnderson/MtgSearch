import { Component, Type } from '@angular/core';
import { INegatableRemovableComponent, INegatableRemovableInput } from '../NegatableRemovable.outletspec';
import { UnSeparatedFormFragmentComponent, UnSepInputs } from '../un-separated-form-fragment/un-separated-form-fragment.component';
import { CharSeparatedFormFragmentComponent, CharSepInputs } from '../char-separated-form-fragment/char-separated-form-fragment.component';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { CardAttributeType } from '../attribute-filter-form-fragment/attribute-filter-form-fragment.component';
import { NameLikeFormFragmentComponent, NameLikeInput } from '../name-like-form-fragment/name-like-form-fragment.component';
import { TypeLineFormFragmentComponent, TypeLineFormInput } from '../type-line-form-fragment/type-line-form-fragment.component';
interface IInputHolder {
  inputs: Record<string, unknown>
  component: Type<any>
  trackingId: string,
  negateAndMoveToOther(otherList: RemovableComponentList): void;
}
class RemovableComponent<T extends INegatableRemovableComponent<I>,I extends INegatableRemovableInput> implements IInputHolder{
  constructor(public trackingId: string, public input: I, public component: Type<T>, private list: RemovableComponentList) {
    this.input.removeClickedChannel.subscribe(() => list.remove(this));
    this.inputs = { input };
  }
  public negateAndMoveToOther(otherList: RemovableComponentList) {
    this.input.isNegative = !this.input.isNegative;
    this.list.remove(this);
    otherList.push(this.component, this.input);
  }
  public inputs: Record<string,unknown>
}
class RemovableComponentList {
  public list: IInputHolder[] = [];
  public remove<T extends INegatableRemovableComponent<I>,I extends INegatableRemovableInput>(item: RemovableComponent<T,I>) {
    let index = this.list.indexOf(item);
    if (index == -1) return;
    this.list.splice(index, 1);
  }
  public push<T extends INegatableRemovableComponent<I>,I extends INegatableRemovableInput>(component: Type<T>, input: I) {
    let id = '' + this.list.length + ':' + new Date().getTime();
    this._push(new RemovableComponent(id, input, component, this));
  }
  public _push<T extends INegatableRemovableComponent<I>,I extends INegatableRemovableInput>(item: RemovableComponent<T,I>) {
    this.list.push(item);
  }
}
class AttributeFilter {
  public constructor(public type: CardAttributeType, private index: number, private filterList: AttributeFilter[]) { }
  public trackingId = crypto.randomUUID();
  public onRemoveRequested() {
    let index = this.filterList.indexOf(this);
    if (index == -1) return;
    this.filterList.splice(index, 1);
  }
}
@Component({
  selector: 'visual-form-flyout',
  templateUrl: './visual-form-flyout.component.html',
  styleUrl: './visual-form-flyout.component.css'
})
export class VisualFormFlyoutComponent {
  public expanded = false;
  public toggleCollapse() {
    this.expanded = !this.expanded;
  }
  public matched = new RemovableComponentList();
  public excluded = new RemovableComponentList();
  public matchUnsep() {
    this.matched.push(UnSeparatedFormFragmentComponent, new UnSepInputs(false));
  }
  public matchCommaSep() {
    this.matched.push(CharSeparatedFormFragmentComponent, new CharSepInputs(false, ','));
  }
  public matchColonSep() {
    this.matched.push(CharSeparatedFormFragmentComponent, new CharSepInputs(false, ':'));
  }
  public matchNameLike() {
    this.matched.push(NameLikeFormFragmentComponent, new NameLikeInput(false));
  }
  public matchSuperTypes() {
    this.matched.push(TypeLineFormFragmentComponent, new TypeLineFormInput(false, "Supertype"));
  }
  public matchTypes() {
    this.matched.push(TypeLineFormFragmentComponent, new TypeLineFormInput(false, "Type"));
  }
  public matchSubTypes() {
    this.matched.push(TypeLineFormFragmentComponent, new TypeLineFormInput(false, "Subtype"));

  }
  public dropped(event: CdkDragDrop<IInputHolder[]>, target: RemovableComponentList) {
    if (event.previousContainer === event.container) return;//don't care about reorder
    let prevIndex = event.previousIndex;
    event.previousContainer.data[prevIndex].negateAndMoveToOther(target);
  }
  public attributeFilters: AttributeFilter[] = [];
  private addAttributeFilter(type: CardAttributeType) {
    this.attributeFilters.push(new AttributeFilter(type, this.attributeFilters.length, this.attributeFilters));
  }
  public addPow() { this.addAttributeFilter(CardAttributeType.pow); }
  public addDef() { this.addAttributeFilter(CardAttributeType.def); }
  public addMv() { this.addAttributeFilter(CardAttributeType.mv); }
  public addLoyalty() { this.addAttributeFilter(CardAttributeType.loyalty); }

}
