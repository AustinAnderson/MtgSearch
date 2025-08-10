import { Component, Input } from "@angular/core";

@Component({
  selector: 'expression-definition',
  templateUrl: './expression-definition.component.html',
  styleUrl: '../selected-documentation-item.css'
})
export class ExpressionDefinitionComponent {
  @Input() public name: string | undefined;
  @Input() public template: string | undefined;
  @Input() public notes: string[] = [];
  @Input() public examples: string[] = [];
}
