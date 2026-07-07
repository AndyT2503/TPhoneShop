import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  inject,
  input,
  model,
  viewChild
} from '@angular/core';
import { LucideCloudUpload, LucideX } from '@lucide/angular';
import { FileAPIService } from '@tphone-shop.web/data-access';
import { finalize } from 'rxjs';
import { LoadingDirective } from '../loading/loading.directive';
import { ToastService } from '../toast';

@Component({
  selector: 'lib-upload-input',
  imports: [LucideCloudUpload, LucideX, LoadingDirective],
  templateUrl: './upload-input.component.html',
  styleUrl: './upload-input.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UploadInputComponent {
  private readonly fileAPIService = inject(FileAPIService);
  private readonly toastService = inject(ToastService);
  private readonly fileInput =
    viewChild<ElementRef<HTMLInputElement>>('fileInput');

  readonly acceptType = input('image/*')
  readonly helperText = input('PNG, JPG hoặc WebP');
  readonly imagePreview = model<{
    presignedUrl: string;
    mediaId: string;
  } | null>(null);

  readonly isUploadingFile = model(false);

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) return;

    if (!file.type.startsWith('image/')) {
      this.toastService.error('Vui lòng chọn file ảnh hợp lệ');
      input.value = '';
      return;
    }

    this.isUploadingFile.set(true);
    this.fileAPIService
      .uploadFile(file)
      .pipe(
        finalize(() => {
          this.isUploadingFile.set(false);
        }),
      )
      .subscribe({
        next: (res) => {
          this.imagePreview.set({
            presignedUrl: res.presignedUrl,
            mediaId: res.mediaId,
          });
        },
        error: (err) => {
          this.toastService.error(
            err.error?.message || 'Tải lên file thất bại',
          );
          input.value = '';
        },
      });
  }

  removeImage(): void {
    this.imagePreview.set(null);
    const input = this.fileInput()?.nativeElement;
    if (input) {
      input.value = '';
    }
  }
}
