import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SsoBtnComponent } from './sso-btn.component';

describe('SsoBtnComponent', () => {
  let component: SsoBtnComponent;
  let fixture: ComponentFixture<SsoBtnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SsoBtnComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SsoBtnComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
