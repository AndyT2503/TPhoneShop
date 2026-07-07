import { ChangeDetectionStrategy, Component, signal } from '@angular/core';

@Component({
  selector: 'lib-table',
  imports: [],
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TableComponent {
  readonly tableData = signal<object[]>([]);
}
