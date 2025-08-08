import { Component, Input } from "@angular/core";

@Component({
  selector: 'expression-definition',
  templateUrl: './expression-definition.component.html',
})
export class ExpressionDefinitionComponent {
  @Input() public name: string | undefined;
  @Input() public template: string | undefined;
  @Input() public notes: string[] = [];
  @Input() public examples: string[] = [];
}
