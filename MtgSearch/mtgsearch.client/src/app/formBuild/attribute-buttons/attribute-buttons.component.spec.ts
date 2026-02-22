import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttributeButtonsComponent } from './attribute-buttons.component';

describe('AttributeButtonsComponent', () => {
  let component: AttributeButtonsComponent;
  let fixture: ComponentFixture<AttributeButtonsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AttributeButtonsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AttributeButtonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
