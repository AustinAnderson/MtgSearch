import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'attribute-buttons',
  templateUrl: './attribute-buttons.component.html',
  styleUrl: './attribute-buttons.component.css'
})
export class AttributeButtonsComponent {
  @Output() nameClicked = new EventEmitter<void>();
  @Output() mvClicked = new EventEmitter<void>();
  @Output() superTypeClicked = new EventEmitter<void>();
  @Output() typeClicked= new EventEmitter<void>();
  @Output() subTypeClicked = new EventEmitter<void>();
  @Output() pureTextClicked = new EventEmitter<void>();
  @Output() commaSepClicked = new EventEmitter<void>();
  @Output() colonSepClicked = new EventEmitter<void>();
  @Output() loyaltyClicked = new EventEmitter<void>();
  @Output() powClicked = new EventEmitter<void>();
  @Output() defClicked = new EventEmitter<void>();

}
