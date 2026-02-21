import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CardComponent } from './card/card.component';
import { CardSymbolComponent } from './symbol/card-symbol.component';
import { ColorIdSelectorComponent } from './colorIdSelector/color-id-selector.component';
import { FunctionDefinitionComponent } from './documentation/functions/function-definition.component';
import { LinkAccordionComponent } from './documentation/accordion/link-accordion.component';
import { DocumentationComponent } from './documentation/documentation.component';
import { ExpressionDefinitionComponent } from './documentation/expressions/expression-definition.component';
import { PlaneswalkerPlaqueComponent } from './symbol/planeswalker-plaque.component';
import { UnSeparatedFormFragmentComponent } from './formBuild/un-separated-form-fragment/un-separated-form-fragment.component';
import { CharSeparatedFormFragmentComponent } from './formBuild/char-separated-form-fragment/char-separated-form-fragment.component';
import { VisualFormFlyoutComponent } from './formBuild/visual-form-flyout/visual-form-flyout.component';
import { CdkDrag, CdkDropList } from '@angular/cdk/drag-drop';
import { AttributeFilterFormFragmentComponent } from './formBuild/attribute-filter-form-fragment/attribute-filter-form-fragment.component';
import { FormDropDownComponent } from './formBuild/form-drop-down/form-drop-down.component';

@NgModule({
  declarations: [
    AppComponent,
    CardComponent,
    CardSymbolComponent,
    ColorIdSelectorComponent,
    DocumentationComponent,
    LinkAccordionComponent,
    FunctionDefinitionComponent,
    ExpressionDefinitionComponent,
    PlaneswalkerPlaqueComponent,
    UnSeparatedFormFragmentComponent,
    CharSeparatedFormFragmentComponent,
    VisualFormFlyoutComponent,
    AttributeFilterFormFragmentComponent,
    FormDropDownComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule,
    CdkDropList, CdkDrag
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
