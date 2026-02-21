import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisualFormFlyoutComponent } from './visual-form-flyout.component';

describe('VisualFormFlyoutComponent', () => {
  let component: VisualFormFlyoutComponent;
  let fixture: ComponentFixture<VisualFormFlyoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [VisualFormFlyoutComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VisualFormFlyoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
