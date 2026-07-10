import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UpdateBrandDialogComponent } from './update-brand-dialog.component';

describe('UpdateBrandDialogComponent', () => {
  let component: UpdateBrandDialogComponent;
  let fixture: ComponentFixture<UpdateBrandDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateBrandDialogComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(UpdateBrandDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
