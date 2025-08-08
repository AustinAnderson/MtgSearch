import { Component, Input } from "@angular/core";

@Component({
  selector: 'function-definition',
  templateUrl: './function-definition.component.html',
})
export class FunctionDefinitionComponent {
  @Input() public name: string = "(unknown)";
  @Input() public description: string[] = [];
  @Input() public signitures: string[] = [];
  @Input() public examples: string[] = [];
}
