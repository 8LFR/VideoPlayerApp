import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadVideoModalComponent } from './upload-video-modal.component';

describe('UploadVideoModalComponent', () => {
  let component: UploadVideoModalComponent;
  let fixture: ComponentFixture<UploadVideoModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UploadVideoModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UploadVideoModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
