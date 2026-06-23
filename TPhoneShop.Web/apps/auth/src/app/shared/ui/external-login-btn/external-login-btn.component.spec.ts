import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ExternalLoginBtnComponent } from './external-login-btn.component';

describe('SsoBtnComponent', () => {
  let component: ExternalLoginBtnComponent;
  let fixture: ComponentFixture<ExternalLoginBtnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExternalLoginBtnComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ExternalLoginBtnComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
