import { Component, Output } from "@angular/core";

export class TextFilter {
  public constructor(public positive: boolean = true, public filter: string = "") { }
}
@Component({
  selector: 'simple-input-form',
  templateUrl: './simple-input-form.component.html',
})
export class SimpleInputForm{
  @Output() public query: string = "";
  public textFilters: TextFilter[] = [];
  public currentPositiveTextFilter: string | undefined;
  public currentNegativeTextFilter: string | undefined;
  public removeTextFilter(index: number) {
    let newFilters: TextFilter[] = [];
    for (let i = 0; i < this.textFilters.length; i++) {
      if (i != index) {
        newFilters.push(this.textFilters[i]);
      }
    }
    this.textFilters = newFilters;
  }
  public clear() {
    this.currentNegativeTextFilter = undefined;
    this.currentPositiveTextFilter = undefined;
    this.textFilters = [];
  }
  public addCurrentNegativeText() {
    this.textFilters.push(new TextFilter(false, this.currentNegativeTextFilter));
    this.UpdateQuery();
  }
  public addCurrentPositiveText() {
    this.textFilters.push(new TextFilter(true, this.currentPositiveTextFilter));
    this.UpdateQuery();
  }
  private UpdateQuery() {
    
  }
}
