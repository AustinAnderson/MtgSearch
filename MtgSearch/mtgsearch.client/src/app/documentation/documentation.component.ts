import { Component, HostBinding } from "@angular/core";
import { LanguageSpec } from "../models/LanguageSpec";
import { FunctionDefinition } from "../models/FunctionDefinition";
import { ExpressionSpec } from "../models/ExpressionSpec";
import { Subject } from "rxjs";


@Component({
  selector: 'documentation',
  templateUrl: './documentation.component.html',
})
export class DocumentationComponent {
  private _spec: LanguageSpec = {
    functionDefinitions: [],
    expressions: { name: "", explainationText: [], template: "", examples:[] },
    isExpressions: { name: "", explainationText: [], template: "", examples: [] }
  };
  public set spec(value: LanguageSpec) {
    const errMsg = "(unable to fetch)";
    this._spec = value;
    this.funcNames = value.functionDefinitions.map(x => x.name ?? errMsg);
    this.expressionNames = [value.expressions?.name ?? errMsg, value.isExpressions?.name ?? errMsg];
  }
  public funcNames: string[] = [];
  public selectedFunction: FunctionDefinition | undefined;

  public expressionNames: string[] = [];
  public selectedExpression: ExpressionSpec | undefined;

  public displayError = false;
  public errorDisplay: string = "An error occured, check the console with ctrl + shift + J";

  @HostBinding("style.--docDisplayPaneBgColor") public dispColor: string = '#F1F1F1';

  public selectionChangedOnAccordion: Subject<string> = new Subject();

  public constructor() {
    this.spec = {
      functionDefinitions: [{
        name: "text",
        description: ["line1", "line2"],
        examples: ["text(\"asdf\")", "text(\"kjlkj\")"],
        signitures: ["text(\"asdf\")", "text(\"kjlkj\")"],
      },
      {
        name: "activated",
        description: ["line1", "line2"],
        examples: ["activated(\"asdf\")", "text(\"kjlkj\")"],
        signitures: ["activated(\"asdf\")", "text(\"kjlkj\")"],
      }],
      expressions: {
        name: "numeric comparisons", explainationText: [
          "where key can be",
          "`mv` or `cmc`: ConvertedManaCost",
          "`pow`: Power",
          "`def` or `toughness`: Toughness",
          "`loyalty`: Loyalty",
          "and operator can be",
          "`>`: GreaterThan",
          "`<`: LessThan",
          "`>=`: GreatherThanOrEquals",
          "`<=`: LessThanOrEquals",
          "`==` or `=`: Equal",
          "and number is a 1-4 digit integer"
        ], template: "{key} {operator} {number}",
        examples: [
          "mv < 3",
          "pow >= 2",
          "def = 5"
        ]
      },
      isExpressions: { name: "assertions", explainationText: [], template: "{key} is *", examples: ["pow is *"]}
    }
  }
  public functionsHeader = "Functions";
  public functionSelectionChanged(name: string) {
    this.selectionChangedOnAccordion.next(this.functionsHeader);
    this.selectedExpression = undefined;
    let filtered = this._spec.functionDefinitions.filter(x => x.name == name);
    if (!filtered || filtered.length != 1) {
      this.selectedFunction = undefined;
      console.log(`unable to fetch function definition for ${name}`);
      this.displayError = true;
    }
    else {
      this.selectedFunction = filtered[0];
    }
  }
  public expressionsHeader = "Expressions";
  public selectedExpressionChanged(name: string) {
    this.selectionChangedOnAccordion.next(this.expressionsHeader);
    this.selectedFunction = undefined;
    if (this._spec.expressions?.name == name) {
      this.selectedExpression = this._spec.expressions;
    }
    else if (this._spec.isExpressions?.name == name) {
      this.selectedExpression = this._spec.isExpressions;
    }
    else {
      console.log(`unable to fetch expression definition for template '${name}'`);
      this.selectedExpression = undefined;
      this.displayError = true;
    }
  }
}
