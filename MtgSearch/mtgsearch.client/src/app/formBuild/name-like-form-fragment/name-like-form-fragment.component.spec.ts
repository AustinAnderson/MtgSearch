import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NameLikeFormFragmentComponent } from './name-like-form-fragment.component';

describe('NameLikeFormFragmentComponent', () => {
  let component: NameLikeFormFragmentComponent;
  let fixture: ComponentFixture<NameLikeFormFragmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [NameLikeFormFragmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NameLikeFormFragmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
