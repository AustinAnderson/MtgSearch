import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormDropDownComponent } from './form-drop-down.component';

describe('FormDropDownComponent', () => {
  let component: FormDropDownComponent;
  let fixture: ComponentFixture<FormDropDownComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FormDropDownComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormDropDownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
