import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AttributeFilterFormFragmentComponent } from './attribute-filter-form-fragment.component';

describe('AttributeFilterFormFragmentComponent', () => {
  let component: AttributeFilterFormFragmentComponent;
  let fixture: ComponentFixture<AttributeFilterFormFragmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AttributeFilterFormFragmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AttributeFilterFormFragmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
