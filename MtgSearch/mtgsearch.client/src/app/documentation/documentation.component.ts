import { Component, HostBinding, Input } from "@angular/core";
import { LanguageSpec } from "../models/LanguageSpec";
import { FunctionDefinition } from "../models/FunctionDefinition";
import { ExpressionSpec } from "../models/ExpressionSpec";
import { Subject } from "rxjs";
import { Note } from "../models/Note";


@Component({
  selector: 'documentation',
  templateUrl: './documentation.component.html',
  styleUrls: ['./documentation-page.css', './selected-documentation-item.css']
})
export class DocumentationComponent {
  private _spec: LanguageSpec = {
    functionDefinitions: [],
    expressions: { name: "", explainationText: [], template: "", examples:[] },
    isExpressions: { name: "", explainationText: [], template: "", examples: [] }
  };
  @Input() public set spec(value: LanguageSpec) {
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
  public generalHeader = "General Notes";
  public selectedNote: Note | undefined;
  public notes: Note[] = [
    new Note("query format", [
      "The search input, or 'query' is made up of one or more filters joined by operators",
      "Operators can be 'and' 'or' 'not' and parenthesis ('&', '|', and '!' are also accepted, as well as '&&' and '||')",
      "Filters take the form of either a function, or an expression (see other sections below)",
      "For example:",
      "(text(\"draw\") and not text(\"discard\")) or mv < 3",
      "which brings back all the cards that cost less than 3, along with cards that say draw but dont say discard"
    ]),
    new Note("string", [
      "A string is anything between double quotes, like \"hello, this is a string\".",
      "If you want to have a double quote in the string,",
      "escape it with a backslash like \"string with a \\\" in it\"",
    ]),
    new Note("regex", [
      "Regex is short for regular expressions, which is a string in a specific format",
      "regex strings have a set of symbols that can be used as short hand for repititions or options",
      "for instance, \"whenever.*draw a card\" will match anything that starts with 'whenever' and ends with 'draw a card'",
      "in practice this can be treated as a normal string,",
      "with the exception of needing to escape reserved symbols like (, +, [, {, or . with a backslash",
      "for more information, checkout regex101.com"
    ]),
    new Note("signiture", [
      "Functions have signitures, which is the template for the way you're allowed to use it.",
      "a signiture starts with the name, followed by zero or more comma separated arguments enclosed by parenthesis",
      "an argument is made up of a descriptive name and a type, separated by colon",
      "where the type is string, regex, int (whole number), or boolean (true or false)",
      "For example:",
      "text(filter: regex)",
      "means there's a function called text which requires you to have a regex string to use as it's filter in the parenthesis",
      "(see more details in the Functions documentation list)",
      "",
      "some functions use the notation ...<type>[]",
      "this just means you can include any number of arguments of that type (see types.all as an example)"
    ])
  ];
  public noteNames: string[] = this.notes.map(x => x.title);
  public selectedNoteChanged(name: string) {
    this.selectionChangedOnAccordion.next(this.generalHeader);
    this.selectedExpression = undefined;
    this.selectedFunction = undefined;
    this.displayError = false;
    for (let note of this.notes) {
      if (note.title == name) {
        this.selectedNote = note;
      }
    }
    if (!this.selectedNote) {
      console.log(`unable to fetch expression definition for template '${name}'`);
      this.displayError = true;
    }
  }
  public functionsHeader = "Functions";
  public functionSelectionChanged(name: string) {
    this.selectionChangedOnAccordion.next(this.functionsHeader);
    this.displayError = false;
    this.selectedExpression = undefined;
    this.selectedNote = undefined;
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
    this.displayError = false;
    this.selectedFunction = undefined;
    this.selectedNote = undefined;
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
