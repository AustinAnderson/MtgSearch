import { ExpressionSpec } from "./ExpressionSpec";
import { FunctionDefinition } from "./FunctionDefinition";

export class LanguageSpec {
  public functionDefinitions: FunctionDefinition[] = [];
  public expressions: ExpressionSpec | undefined;
  public isExpressions: ExpressionSpec | undefined;
}
