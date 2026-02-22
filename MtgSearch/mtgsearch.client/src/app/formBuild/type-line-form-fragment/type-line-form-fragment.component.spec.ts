import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TypeLineFormFragmentComponent } from './type-line-form-fragment.component';

describe('TypeLineFormFragmentComponent', () => {
  let component: TypeLineFormFragmentComponent;
  let fixture: ComponentFixture<TypeLineFormFragmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TypeLineFormFragmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TypeLineFormFragmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
