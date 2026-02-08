import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnSeparatedFormFragmentComponent } from './un-separated-form-fragment.component';

describe('UnSeparatedFormFragmentComponent', () => {
  let component: UnSeparatedFormFragmentComponent;
  let fixture: ComponentFixture<UnSeparatedFormFragmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UnSeparatedFormFragmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UnSeparatedFormFragmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
