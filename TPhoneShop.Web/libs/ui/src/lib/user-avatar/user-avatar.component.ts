import {
  ChangeDetectionStrategy,
  Component,
  computed,
  input,
} from '@angular/core';

@Component({
  selector: 'lib-user-avatar',
  imports: [],
  templateUrl: './user-avatar.component.html',
  styleUrl: './user-avatar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserAvatarComponent {
  readonly fullName = input.required<string>();
  readonly size = input<'sm' | 'md' | 'lg'>('sm');

  readonly firstLetter = computed(() => this.fullName()[0].toUpperCase());
}
