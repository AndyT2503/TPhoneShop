import { ChangeDetectionStrategy, Component, input, model, output } from '@angular/core';

export interface StatusTabCount {
  active: number;
  inactive: number;
}

@Component({
  selector: 'app-status-filter',
  imports: [],
  templateUrl: './status-filter.component.html',
  styleUrl: './status-filter.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StatusFilterComponent {
  readonly isActive = model<boolean>(false);

  readonly tabs = [
    { key: true as const, label: 'Đang hoạt động' },
    { key: false as const, label: 'Ngừng hoạt động' },
  ];

  onTabSelect(key: boolean): void {
    this.isActive.set(key);
  }
}
