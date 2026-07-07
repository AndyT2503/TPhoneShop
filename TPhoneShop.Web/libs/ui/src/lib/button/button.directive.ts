import { computed, Directive, input } from '@angular/core';

type ButtonSize = 'sm' | 'md' | 'lg';

const BASE_CLASSES =
  'inline-flex items-center gap-1 rounded-lg font-medium text-white transition-colors';

const ENABLED_CLASSES =
  'bg-indigo-600 hover:bg-indigo-700 cursor-pointer';

const DISABLED_CLASSES =
  'bg-gray-400 cursor-not-allowed opacity-50';

const SIZE_CLASSES: Record<ButtonSize, string> = {
  sm: 'px-3 py-1.5 text-xs',
  md: 'px-4 py-2 text-sm',
  lg: 'px-5 py-2.5 text-base',
};

@Directive({
  selector: '[libButton]',
  host: {
    '[class]': 'classes()',
    '[attr.disabled]': 'disabled() ? "" : null',
    '[attr.aria-disabled]': 'disabled()',
  },
})
export class ButtonDirective {
  readonly size = input<ButtonSize>('sm');
  readonly disabled = input(false);

  readonly classes = computed(() => {
    const stateClasses = this.disabled()
      ? DISABLED_CLASSES
      : ENABLED_CLASSES;

    return `${BASE_CLASSES} ${SIZE_CLASSES[this.size()]} ${stateClasses}`;
  });
}
