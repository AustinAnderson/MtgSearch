import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CharSeparatedFormFragmentComponent } from './char-separated-form-fragment.component';

describe('CharSeparatedFormFragmentComponent', () => {
  let component: CharSeparatedFormFragmentComponent;
  let fixture: ComponentFixture<CharSeparatedFormFragmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CharSeparatedFormFragmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CharSeparatedFormFragmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
