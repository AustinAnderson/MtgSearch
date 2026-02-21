import { EventEmitter } from "@angular/core";

export interface INegatableRemovableComponent<T extends INegatableRemovableInput> {
  input: T
}
export interface INegatableRemovableInput {
  removeClickedChannel: EventEmitter<void>
  isNegative: boolean;
}
