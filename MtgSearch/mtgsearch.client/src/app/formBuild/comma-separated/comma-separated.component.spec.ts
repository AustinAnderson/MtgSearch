import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommaSeparatedComponent } from './comma-separated.component';

describe('CommaSeparatedComponent', () => {
  let component: CommaSeparatedComponent;
  let fixture: ComponentFixture<CommaSeparatedComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CommaSeparatedComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommaSeparatedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
