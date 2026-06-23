import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ShopLogoComponent } from './shop-logo.component';

describe('ShopLogoComponent', () => {
  let component: ShopLogoComponent;
  let fixture: ComponentFixture<ShopLogoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShopLogoComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ShopLogoComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
