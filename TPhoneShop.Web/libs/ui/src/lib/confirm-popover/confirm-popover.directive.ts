import {
  Directive,
  ElementRef,
  HostListener,
  inject,
  input,
  output,
  Renderer2
} from '@angular/core';

@Directive({
  selector: '[libConfirmPopover]',
  standalone: true,
})
export class ConfirmPopoverDirective {
  private readonly el = inject(ElementRef);
  private readonly renderer = inject(Renderer2)
  private popoverEl: HTMLElement | null = null;

  readonly message = input('Bạn có chắc chắn?');
  readonly confirm = output<void>();
  // eslint-disable-next-line @angular-eslint/no-output-native
  readonly cancel = output<void>();



  @HostListener('click')
  handleClick() {
    if (this.popoverEl) {
      this.destroyPopover();
      return;
    }

    this.createPopover();
  }

  @HostListener('window:resize')
  handleResize() {
    this.destroyPopover();
  }

  private createPopover() {
    const host = this.el.nativeElement as HTMLElement;
    const popover = this.renderer.createElement('div') as HTMLElement;

    this.popoverEl = popover;

    this.renderer.addClass(popover, 'fixed');
    this.renderer.addClass(popover, 'z-10000');
    this.renderer.addClass(popover, 'rounded-xl');
    this.renderer.addClass(popover, 'border');
    this.renderer.addClass(popover, 'border-indigo-600');
    this.renderer.addClass(popover, 'bg-white');
    this.renderer.addClass(popover, 'shadow-lg');
    this.renderer.setStyle(popover, 'minWidth', '240px');
    this.renderer.setStyle(popover, 'width', 'max-content');
    this.renderer.setStyle(popover, 'maxWidth', 'calc(100vw - 16px)');
    this.renderer.setStyle(popover, 'boxSizing', 'border-box');
    this.renderer.setStyle(popover, 'overflow', 'hidden');

    popover.innerHTML = `
      <div style="padding:12px">
        <p style="margin-bottom:10px;font-size:14px;line-height:20px;word-break:break-word;">${this.message()}</p>
        <div style="display:flex;justify-content:flex-end;gap:8px;flex-wrap:wrap;">
          <button data-cancel style="cursor: pointer; width: 70px;border:1px solid #ddd;border-radius:6px;flex-shrink:0; font-size: 14px;">Hủy</button>
          <button data-ok style="cursor: pointer;width: 70px;background:#4f39f6;color:#fff;border-radius:6px;flex-shrink:0;font-size: 14px;">OK</button>
        </div>
      </div>
    `;

    document.body.appendChild(popover);

    const rect = host.getBoundingClientRect();
    const popoverWidth = popover.offsetWidth;
    const popoverHeight = popover.offsetHeight;
    const viewportPadding = 8;
    const offset = 8;
    const viewportWidth = window.innerWidth;
    const viewportHeight = window.innerHeight;

    let left = rect.right - popoverWidth;
    let top = rect.top - popoverHeight - offset;

    if (left < viewportPadding) {
      left = viewportPadding;
    }

    if (left + popoverWidth > viewportWidth - viewportPadding) {
      left = viewportWidth - popoverWidth - viewportPadding;
    }

    if (top < viewportPadding) {
      top = rect.bottom + offset;
    }

    if (top + popoverHeight > viewportHeight - viewportPadding) {
      top = Math.max(viewportPadding, viewportHeight - popoverHeight - viewportPadding);
    }

    this.renderer.setStyle(popover, 'top', `${top}px`);
    this.renderer.setStyle(popover, 'left', `${left}px`);

    popover.querySelector('[data-ok]')?.addEventListener('click', () => {
      this.confirm.emit();
      this.destroyPopover();
    });

    popover.querySelector('[data-cancel]')?.addEventListener('click', () => {
      this.cancel.emit();
      this.destroyPopover();
    });

    setTimeout(() => {
      document.addEventListener('pointerdown', this.handleOutsideClick);
    });
  }

  private handleOutsideClick = (event: MouseEvent) => {
    if (!this.popoverEl) {
      return;
    }

    if (
      this.popoverEl.contains(event.target as Node) ||
      this.el.nativeElement.contains(event.target)
    ) {
      return;
    }

    this.destroyPopover();
  };

  private destroyPopover() {
    if (!this.popoverEl) {
      return;
    }

    document.body.removeChild(this.popoverEl);
    this.popoverEl = null;
    document.removeEventListener('click', this.handleOutsideClick);
  }
}
