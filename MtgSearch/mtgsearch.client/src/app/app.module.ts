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
    PlaneswalkerPlaqueComponent 
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
